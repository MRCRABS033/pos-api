namespace Pos.Domain.Entities;

public enum PaymentType
{
    Cash,
    Card
}
public class Sale
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }
    //campo agregado para identificar el tipo de pago.
    public PaymentType PaymentType { get; set; }
    public virtual User User { get; set; } = default!;
    public decimal Total { get; set; }
    public decimal Discount { get; set; }
    public Guid? CashBoxId { get; set; }
    public virtual CashBox? CashBox { get; set; }
    public virtual ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
}
