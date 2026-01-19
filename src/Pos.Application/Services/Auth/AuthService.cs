using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pos.Application.Configuration;
using Pos.Application.Dtos.Auth;
using Pos.Application.Interfaces.Services;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtOptions)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<AuthResponseDto> RegisterAsync(AuthRegisterDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "El usuario no puede ser nulo.");

        var existing = await _userRepository.GetByEmail(dto.Email);
        if (existing != null)
            throw new InvalidOperationException("El correo ya esta registrado.");

        var now = DateTime.UtcNow;
        var user = new User
        {
            Name = dto.Name,
            LastName = dto.LastName,
            NormaliceName = NormalizeName(dto.Name, dto.LastName),
            Email = dto.Email,
            Phone = dto.Phone,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role,
            IsActive = true,
            Created = now,
            Modified = now
        };

        var created = await _userRepository.CreateAsync(user);
        var token = GenerateToken(created);

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

        var token = GenerateToken(user);

        return new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            Name = user.Name,
            LastName = user.LastName,
            Token = token
        };
    }

    private string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new("userId", user.Id.ToString()),
            new(ClaimTypes.Role, user.Role)
        };

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
