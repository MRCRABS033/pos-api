using Pos.Application.Dtos.SaleItems;

namespace Pos.Application.Dtos.Sales;

public class SaleCreateDto
{
    public List<SaleItemCreateDto> Items { get; set; } = new();
}
