namespace Pos.Domain.Entities;

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
    public bool IsOwner { get; set; }
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public virtual ICollection<CashBox> CashBoxes { get; set; } = new List<CashBox>();
    public virtual ICollection<CashFlow> CashFlows { get; set; } = new List<CashFlow>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    public virtual ICollection<Return> Returns { get; set; } = new List<Return>();
}
