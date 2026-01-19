namespace Pos.Domain.Entities;

[Microsoft.EntityFrameworkCore.Index(nameof(Email), IsUnique = true)]
[Microsoft.EntityFrameworkCore.Index(nameof(NormaliceName), IsUnique = true)]
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string LastName { get; set; }
    public string NormaliceName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = "User";
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public ICollection<CashFlow> CashFlows { get; set; } = new List<CashFlow>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
