namespace Pos.Application.Dtos.Auth;

public class AuthResponseDto
{
    public Guid UserId { get; set; }
    public string? Email { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
