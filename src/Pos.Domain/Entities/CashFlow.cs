namespace Pos.Domain.Entities;

public class CashFlow
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Motive {get; set;}
    public decimal Amount { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
}
