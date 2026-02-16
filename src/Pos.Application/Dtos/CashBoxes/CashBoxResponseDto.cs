using Pos.Domain.Entities;

namespace Pos.Application.Dtos.CashBoxes;

public class CashBoxResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal CashTotal { get; set; }
    public decimal CardTotal { get; set; }
    public decimal ExpectatedBalance { get; set; }
    public decimal ActualBalance { get; set; }
    public int TicketsCount { get; set; }
}
