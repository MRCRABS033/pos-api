namespace Pos.Application.Dtos.Users;

public class UserUpdateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
    public string? Password { get; set; }
    public string Role { get; set; } = "User";
}
