using Pos.Application.Dtos.SaleItems;

namespace Pos.Application.Dtos.Sales;

public class SaleUpdateDto
{
    public Guid Id { get; set; }
    public List<SaleItemUpdateDto> Items { get; set; } = new();
}
