using BCrypt.Net;
using Pos.Application.Dtos.Users;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Users;

public class UpdateUserUseCase
{
    private readonly IUserRepository _userRepository;

    public UpdateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto> ExecuteAsync(UserUpdateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "El usuario no puede ser nulo.");

        var current = await _userRepository.GetByIdAsync(dto.Id);
        var password = string.IsNullOrWhiteSpace(dto.Password)
            ? current.Password
            : BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Id = dto.Id,
            Name = dto.Name,
            LastName = dto.LastName,
            NormaliceName = NormalizeName(dto.Name, dto.LastName),
            Email = dto.Email,
            Phone = dto.Phone,
            Password = password,
            Role = dto.Role,
            IsActive = dto.IsActive,
            Created = current.Created,
            Modified = DateTime.UtcNow
        };

        var updated = await _userRepository.UpdateAsync(user);
        return Map(updated);
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
