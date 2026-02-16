namespace Pos.Domain.Entities;

public enum CashFlowType
{
    Entry = 1,
    Exit = 2
}

public class CashFlow
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Motive {get; set;}
    public decimal Amount { get; set; }
    public CashFlowType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public Guid? CashBoxId { get; set; }
    public virtual CashBox? CashBox { get; set; }
}
