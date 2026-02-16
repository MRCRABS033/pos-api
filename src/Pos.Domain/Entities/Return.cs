namespace Pos.Domain.Entities;

public class Return
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SaleId { get; set; }
    public Sale Sale { get; set; } = default!;
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string Reason { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public PaymentType PaymentType { get; set; }
    public virtual ICollection<ReturnItem> Items { get; set; } = new List<ReturnItem>();
}
