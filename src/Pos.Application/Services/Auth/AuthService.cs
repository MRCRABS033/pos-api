using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pos.Application.Configuration;
using Pos.Application.Dtos.Auth;
using Pos.Application.Interfaces.Infrastructure;
using Pos.Application.Interfaces.Services;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;
    private readonly ITransactionalExecutor _transactionalExecutor;

    public AuthService(
        IUserRepository userRepository,
        IOptions<JwtSettings> jwtOptions,
        ITransactionalExecutor transactionalExecutor)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtOptions.Value;
        _transactionalExecutor = transactionalExecutor;
    }

    public async Task<AuthResponseDto> RegisterAsync(AuthRegisterDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "El usuario no puede ser nulo.");

        var created = await _transactionalExecutor.ExecuteAsync(async () =>
        {
            var existing = await _userRepository.GetByEmail(dto.Email);
            if (existing != null)
                throw new InvalidOperationException("El correo ya esta registrado.");

            var hasOwner = await _userRepository.ExistsOwnerAsync();
            var now = DateTime.UtcNow;
            var user = new User
            {
                Name = dto.Name,
                LastName = dto.LastName,
                NormaliceName = NormalizeName(dto.Name, dto.LastName),
                Email = dto.Email,
                Phone = dto.Phone,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = hasOwner ? "User" : "Admin",
                IsOwner = !hasOwner,
                IsActive = true,
                Created = now,
                Modified = now
            };

            var created = await _userRepository.CreateAsync(user);
            await _userRepository.SetDefaultPermissionsByRole(created.Id, created.Role);
            return created;
        });

        var permissions = await _userRepository.GetPermissionCodes(created.Id);
        var token = GenerateToken(created, permissions);

        return new AuthResponseDto
        {
            UserId = created.Id,
            Email = created.Email,
            Name = created.Name,
            LastName = created.LastName,
            Token = token
        };
    }

    public async Task<AuthResponseDto> LoginAsync(AuthLoginDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "Credenciales invalidas.");

        var user = await _userRepository.GetByEmail(dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            throw new UnauthorizedAccessException("Credenciales invalidas.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Usuario inactivo.");

        var permissions = await _userRepository.GetPermissionCodes(user.Id);
        var token = GenerateToken(user, permissions);

        return new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            Name = user.Name,
            LastName = user.LastName,
            Token = token
        };
    }

    private string GenerateToken(User user, IReadOnlyList<string> permissions)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new("userId", user.Id.ToString()),
            new("isOwner", user.IsOwner ? "true" : "false"),
            new(ClaimTypes.Role, user.Role)
        };

        foreach (var permission in permissions)
        {
            claims.Add(new Claim("perm", permission));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string NormalizeName(string name, string lastName)
    {
        return $"{name} {lastName}".Trim().ToLowerInvariant();
    }
}
