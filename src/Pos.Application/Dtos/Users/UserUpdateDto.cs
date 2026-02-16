using System.ComponentModel.DataAnnotations;

namespace Pos.Application.Dtos.Users;

public class UserUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    [MaxLength(80)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [MaxLength(40)]
    public string? Phone { get; set; }
    public bool IsActive { get; set; }

    [MinLength(8)]
    [MaxLength(200)]
    public string? Password { get; set; }
    public string Role { get; set; } = "User";
}
