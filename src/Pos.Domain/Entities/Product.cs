namespace Pos.Domain.Entities;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Sku { get; set; }
    public decimal PriceCost { get; set; }
    public decimal PriceSell  { get; set; }
    public decimal Stock {get; set;}
    public bool IsActive { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; } = default!;
    public Guid UpdatedById { get; set; }
    public User UpdatedBy { get; set; } = default!;
    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    public virtual Category? Category { get; set; }
    public Guid? CategoryId { get; set; }
}
