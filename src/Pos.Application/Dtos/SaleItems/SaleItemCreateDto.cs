namespace Pos.Application.Dtos.SaleItems;

public class SaleItemCreateDto
{
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
