using Pos.Domain.Entities;

namespace Pos.Application.Dtos.CashBoxes;

public class CashBoxCreateDto
{
    public Status Status { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal CashTotal { get; set; }
    public decimal CardTotal { get; set; }
    public decimal ExpectatedBalance { get; set; }
    public decimal ActualBalance { get; set; }
    public int TicketsCount { get; set; }
}
