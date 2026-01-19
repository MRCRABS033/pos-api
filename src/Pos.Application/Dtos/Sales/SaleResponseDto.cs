using Pos.Application.Dtos.SaleItems;

namespace Pos.Application.Dtos.Sales;

public class SaleResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<SaleItemResponseDto> Items { get; set; } = new();
}
