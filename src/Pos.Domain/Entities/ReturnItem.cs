namespace Pos.Domain.Entities;

public class ReturnItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ReturnId { get; set; }
    public Return Return { get; set; } = default!;
    public Guid SaleItemId { get; set; }
    public SaleItem SaleItem { get; set; } = default!;
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
