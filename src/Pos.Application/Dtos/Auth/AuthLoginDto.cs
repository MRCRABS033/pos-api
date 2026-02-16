using System.ComponentModel.DataAnnotations;

namespace Pos.Application.Dtos.Auth;

public class AuthLoginDto
{
    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    [MaxLength(200)]
    public string Password { get; set; } = string.Empty;
}
