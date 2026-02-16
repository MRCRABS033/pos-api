using System.ComponentModel.DataAnnotations;

namespace Pos.Application.Dtos.Returns;

public class ReturnCreateDto
{
    [MinLength(1)]
    public List<ReturnItemCreateDto> Items { get; set; } = new();

    [Required]
    [MinLength(3)]
    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;
}
