using Pos.Application.Dtos.Categories;

namespace Pos.Application.Dtos.Products;

public class ProductResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal PriceCost { get; set; }
    public decimal PriceSell { get; set; }
    public decimal Stock { get; set; }
    public bool IsActive { get; set; }
    public bool IsAvailable { get; set; }
    public Guid CategoryId { get; set; }
    public CategoryResponseDto? Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
