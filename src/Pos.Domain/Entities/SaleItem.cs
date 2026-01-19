namespace Pos.Domain.Entities;

public class SaleItem
{
    public Guid Id { get; set; } =  Guid.NewGuid();
    public Guid SaleId { get; set; }
    public Sale Sale { get; set; } = default!;
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
