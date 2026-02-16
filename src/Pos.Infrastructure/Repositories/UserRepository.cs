using Microsoft.EntityFrameworkCore;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;
using Pos.Domain.Security;
using Pos.Infrastructure.Data;

namespace Pos.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PosDbContext _context;

    public UserRepository(PosDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "El usuario no puede ser nulo.");
        }

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id), "El id del usuario no puede ser nulo.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            throw new KeyNotFoundException("El usuario que buscas no existe en la base de datos.");

        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        var existing = _context.Users.Local.FirstOrDefault(u => u.Id == user.Id)
                       ?? await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        if (existing is null)
            throw new KeyNotFoundException("Usuario no encontrado.");

        existing.Name = user.Name;
        existing.LastName = user.LastName;
        existing.NormaliceName = user.NormaliceName;
        existing.Phone = user.Phone;
        existing.Email = user.Email;
        existing.Password = user.Password;
        existing.Role = user.Role;
        existing.IsOwner = user.IsOwner;
        existing.IsActive = user.IsActive;
        existing.Modified = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
            throw new KeyNotFoundException("Usuario no encontrado.");
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByNormaliceName(string normaliceName)
    {
        return await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.NormaliceName == normaliceName);
    }

    public async Task<bool> ExistsOwnerAsync()
    {
        return await _context.Users.AsNoTracking().AnyAsync(u => u.IsOwner);
    }

    public async Task EnsurePermissionCatalogAsync()
    {
        var existingCodes = (await _context.Permissions.AsNoTracking()
                .Select(p => p.Code)
                .ToListAsync())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var missing = PermissionCodes.All
            .Where(code => !existingCodes.Contains(code))
            .Select(code => new Permission
            {
                Code = code,
                Description = code
            })
            .ToList();

        if (missing.Count == 0)
        {
            await EnsureDefaultPermissionsForUsersWithoutAssignmentsAsync();
            return;
        }

        await _context.Permissions.AddRangeAsync(missing);
        await _context.SaveChangesAsync();
        await EnsureDefaultPermissionsForUsersWithoutAssignmentsAsync();
    }

    public async Task<IReadOnlyList<string>> GetAllPermissionCodesAsync()
    {
        return await _context.Permissions.AsNoTracking()
            .OrderBy(p => p.Code)
            .Select(p => p.Code)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetPermissionCodes(Guid userId)
    {
        var user = await _context.Users.AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new { u.Role, u.IsOwner })
            .FirstOrDefaultAsync();

        if (user is null)
            throw new KeyNotFoundException("Usuario no encontrado.");

        if (user.IsOwner || string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase))
            return await GetAllPermissionCodesAsync();

        return await _context.UserPermissions.AsNoTracking()
            .Where(up => up.UserId == userId)
            .Select(up => up.Permission.Code)
            .OrderBy(code => code)
            .ToListAsync();
    }

    public Task SetDefaultPermissionsByRole(Guid userId, string role)
    {
        var defaults = PermissionProfiles.GetDefaultForRole(role);
        return SetPermissionCodes(userId, defaults);
    }

    private async Task EnsureDefaultPermissionsForUsersWithoutAssignmentsAsync()
    {
        var usersWithoutPermissions = await _context.Users.AsNoTracking()
            .Where(u => !_context.UserPermissions.Any(up => up.UserId == u.Id))
            .Select(u => new { u.Id, u.Role })
            .ToListAsync();

        if (usersWithoutPermissions.Count == 0)
            return;

        var permissionsByCode = await _context.Permissions.AsNoTracking()
            .ToDictionaryAsync(p => p.Code, p => p.Id, StringComparer.OrdinalIgnoreCase);

        var userPermissionsToAdd = new List<UserPermission>();
        foreach (var user in usersWithoutPermissions)
        {
            var defaults = PermissionProfiles.GetDefaultForRole(user.Role);
            foreach (var code in defaults)
            {
                if (permissionsByCode.TryGetValue(code, out var permissionId))
                {
                    userPermissionsToAdd.Add(new UserPermission
                    {
                        UserId = user.Id,
                        PermissionId = permissionId
                    });
                }
            }
        }

        if (userPermissionsToAdd.Count == 0)
            return;

        await _context.UserPermissions.AddRangeAsync(userPermissionsToAdd);
        await _context.SaveChangesAsync();
    }

    public async Task SetPermissionCodes(Guid userId, IReadOnlyList<string> codes)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            throw new KeyNotFoundException("Usuario no encontrado.");

        var normalized = codes
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => c.Trim().ToLowerInvariant())
            .Where(c => c.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        await EnsurePermissionCatalogAsync();

        var existingPermissions = await _context.Permissions
            .Where(p => normalized.Contains(p.Code))
            .ToListAsync();

        var existingCodes = existingPermissions
            .Select(p => p.Code)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var invalidCodes = normalized
            .Where(code => !existingCodes.Contains(code))
            .ToList();

        if (invalidCodes.Count > 0)
            throw new InvalidOperationException($"Permisos inválidos: {string.Join(", ", invalidCodes)}.");

        var permissionIds = existingPermissions
            .Where(p => normalized.Contains(p.Code))
            .Select(p => p.Id)
            .ToHashSet();

        var current = await _context.UserPermissions
            .Where(up => up.UserId == userId)
            .ToListAsync();

        var toRemove = current
            .Where(up => !permissionIds.Contains(up.PermissionId))
            .ToList();

        if (toRemove.Count > 0)
            _context.UserPermissions.RemoveRange(toRemove);

        var currentIds = current.Select(up => up.PermissionId).ToHashSet();
        var toAdd = permissionIds
            .Where(id => !currentIds.Contains(id))
            .Select(id => new UserPermission { UserId = userId, PermissionId = id })
            .ToList();

        if (toAdd.Count > 0)
            _context.UserPermissions.AddRange(toAdd);

        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(int page = 1, int pageSize = 50)
    {
        var normalizedPage = Math.Max(1, page);
        var normalizedSize = Math.Clamp(pageSize, 1, 200);

        return await _context.Users.AsNoTracking()
            .OrderBy(u => u.Name)
            .Skip((normalizedPage - 1) * normalizedSize)
            .Take(normalizedSize)
            .ToListAsync();
    }
}
