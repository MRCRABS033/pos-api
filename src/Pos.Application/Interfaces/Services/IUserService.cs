using Pos.Application.Dtos.Users;

namespace Pos.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserResponseDto> CreateAsync(UserCreateDto dto);
    Task<UserResponseDto> UpdateAsync(UserUpdateDto dto);
    Task<UserResponseDto> GetByIdAsync(Guid id);
    Task<UserResponseDto?> GetByEmailAsync(string email);
    Task<UserResponseDto?> GetByNormaliceNameAsync(string normaliceName);
    Task<IReadOnlyList<UserResponseDto>> GetAllAsync(int page = 1, int pageSize = 50);
    Task DeleteAsync(Guid id);
    Task<UserPermissionsResponseDto> GetPermissionsAsync(Guid userId);
    Task<UserPermissionsResponseDto> UpdatePermissionsAsync(Guid actorUserId, Guid userId, UserPermissionsUpdateDto dto);
}
