namespace Pos.Application.Dtos.CashBoxes;

public class EmployeeShiftSummaryDto
{
    public Guid CashBoxId { get; set; }
    public Guid UserId { get; set; }
    public int TotalActivities { get; set; }
    public int ProductsCreated { get; set; }
    public int ProductsUpdated { get; set; }
    public decimal InventoryAddedTotal { get; set; }
    public decimal InventoryRemovedTotal { get; set; }
    public int CashEntriesCount { get; set; }
    public int CashExitsCount { get; set; }
    public decimal CashEntriesTotal { get; set; }
    public decimal CashExitsTotal { get; set; }
    public IReadOnlyList<ActivityLogItemDto> Activities { get; set; } = Array.Empty<ActivityLogItemDto>();
}
