using Microsoft.EntityFrameworkCore;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;
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

        var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        if (existing is null)
            throw new KeyNotFoundException("Usuario no encontrado.");

        existing.Name = user.Name;
        existing.LastName = user.LastName;
        existing.NormaliceName = user.NormaliceName;
        existing.Phone = user.Phone;
        existing.Email = user.Email;
        existing.Password = user.Password;
        existing.Role = user.Role;
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

    public async Task<IReadOnlyList<User>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking()
            .OrderBy(u => u.Name)
            .Take(100)
            .ToListAsync();
    }
}
