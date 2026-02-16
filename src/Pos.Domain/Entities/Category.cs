namespace Pos.Domain.Entities;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
