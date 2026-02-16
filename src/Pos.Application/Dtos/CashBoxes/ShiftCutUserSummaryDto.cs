namespace Pos.Application.Dtos.CashBoxes;

public class ShiftCutUserSummaryDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int TotalActivities { get; set; }
    public int ProductsCreated { get; set; }
    public int ProductsUpdated { get; set; }
    public decimal InventoryAddedTotal { get; set; }
    public decimal InventoryRemovedTotal { get; set; }
    public decimal CashEntriesTotal { get; set; }
    public decimal CashExitsTotal { get; set; }
}
