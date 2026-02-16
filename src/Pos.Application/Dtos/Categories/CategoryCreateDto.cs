using System.ComponentModel.DataAnnotations;

namespace Pos.Application.Dtos.Categories;

public class CategoryCreateDto
{
    [Required]
    [MinLength(2)]
    [MaxLength(80)]
    public string Name { get; set; } = string.Empty;
}
