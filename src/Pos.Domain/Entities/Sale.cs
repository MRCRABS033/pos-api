namespace Pos.Domain.Entities;

public class Sale
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public decimal Total { get; set; }
    public decimal Discount { get; set; }
    public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
}
