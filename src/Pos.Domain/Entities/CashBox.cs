namespace Pos.Domain.Entities;

public enum Status
{
    Open,
    Closed,
}
public class CashBox
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    // Totales
    public decimal OpeningBalance { get; set; }
    public decimal CashTotal { get; set; }
    public decimal CardTotal { get; set; }
    
    //balance esperado
    public decimal ExpectatedBalance { get; set; }
    //balance real contado por el usuario.
    public decimal ActualBalance { get; set; }

    public int TicketsCount { get; set; }
    public virtual ICollection<CashFlow> CashFlows { get; set; }
    public virtual ICollection<Sale> Sales { get; set; }
}
