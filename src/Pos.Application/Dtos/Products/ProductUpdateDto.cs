using System.ComponentModel.DataAnnotations;

namespace Pos.Application.Dtos.Products;

public class ProductUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    [MaxLength(64)]
    public string Sku { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "999999999")]
    public decimal PriceCost { get; set; }

    [Range(typeof(decimal), "0", "999999999")]
    public decimal PriceSell { get; set; }

    [Range(typeof(decimal), "0", "999999999")]
    public decimal Stock { get; set; }
    public bool IsActive { get; set; }
    public bool IsAvailable { get; set; }
    public Guid? CategoryId { get; set; }
}
