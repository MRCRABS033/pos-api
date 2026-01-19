using Pos.Application.Dtos.Users;

namespace Pos.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserResponseDto> CreateAsync(UserCreateDto dto);
    Task<UserResponseDto> UpdateAsync(UserUpdateDto dto);
    Task<UserResponseDto> GetByIdAsync(Guid id);
    Task<UserResponseDto?> GetByEmailAsync(string email);
    Task<UserResponseDto?> GetByNormaliceNameAsync(string normaliceName);
    Task<IReadOnlyList<UserResponseDto>> GetAllAsync();
    Task DeleteAsync(Guid id);
}
