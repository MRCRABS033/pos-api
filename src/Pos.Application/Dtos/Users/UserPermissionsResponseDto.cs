namespace Pos.Application.Dtos.Users;

public class UserPermissionsResponseDto
{
    public Guid UserId { get; set; }
    public List<string> Permissions { get; set; } = new();
}
