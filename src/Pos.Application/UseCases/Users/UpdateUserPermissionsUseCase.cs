using Pos.Application.Dtos.Users;
using Pos.Domain.Interfaces.Repositories;
using Pos.Domain.Security;

namespace Pos.Application.UseCases.Users;

public class UpdateUserPermissionsUseCase
{
    private readonly IUserRepository _userRepository;

    public UpdateUserPermissionsUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserPermissionsResponseDto> ExecuteAsync(Guid actorUserId, Guid userId, UserPermissionsUpdateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "Los permisos no pueden ser nulos.");

        var actor = await _userRepository.GetByIdAsync(actorUserId);
        var actorPermissions = await _userRepository.GetPermissionCodes(actorUserId);
        var canManage = actor.IsOwner || actorPermissions.Contains(PermissionCodes.PermissionsManage, StringComparer.OrdinalIgnoreCase);
        if (!canManage)
            throw new UnauthorizedAccessException("No tienes permisos para otorgar o revocar permisos.");

        var targetUser = await _userRepository.GetByIdAsync(userId);
        if (targetUser.IsOwner)
            throw new InvalidOperationException("No se pueden modificar los permisos del owner.");

        await _userRepository.SetPermissionCodes(userId, dto.Permissions);
        var updated = await _userRepository.GetPermissionCodes(userId);

        return new UserPermissionsResponseDto
        {
            UserId = userId,
            Permissions = updated.ToList()
        };
    }
}
