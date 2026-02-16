using Pos.Application.Dtos.Users;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Users;

public class GetUserPermissionsUseCase
{
    private readonly IUserRepository _userRepository;

    public GetUserPermissionsUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserPermissionsResponseDto> ExecuteAsync(Guid userId)
    {
        var permissions = await _userRepository.GetPermissionCodes(userId);
        return new UserPermissionsResponseDto
        {
            UserId = userId,
            Permissions = permissions.ToList()
        };
    }
}
