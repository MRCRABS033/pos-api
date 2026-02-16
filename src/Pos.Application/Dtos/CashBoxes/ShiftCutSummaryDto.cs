using Pos.Domain.Entities;

namespace Pos.Application.Dtos.CashBoxes;

public class ShiftCutSummaryDto
{
    public Guid CashBoxId { get; set; }
    public Guid OwnerUserId { get; set; }
    public Status Status { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal CashTotal { get; set; }
    public decimal CardTotal { get; set; }
    public decimal ExpectedBalance { get; set; }
    public decimal ActualBalance { get; set; }
    public int TicketsCount { get; set; }
    public IReadOnlyList<ShiftCutUserSummaryDto> Users { get; set; } = Array.Empty<ShiftCutUserSummaryDto>();
}
