using BCrypt.Net;
using Pos.Application.Dtos.Users;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Users;

public class CreateUserUseCase
{
    private readonly IUserRepository _userRepository;

    public CreateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto> ExecuteAsync(UserCreateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "El usuario no puede ser nulo.");

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
            IsActive = dto.IsActive,
            Created = now,
            Modified = now
        };

        var created = await _userRepository.CreateAsync(user);
        return Map(created);
    }

    private static string NormalizeName(string name, string lastName)
    {
        return $"{name} {lastName}".Trim().ToLowerInvariant();
    }

    private static UserResponseDto Map(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            NormaliceName = user.NormaliceName,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role,
            IsActive = user.IsActive,
            Created = user.Created,
            Modified = user.Modified
        };
    }
}
