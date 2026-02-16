namespace Pos.Domain.Entities;

public enum UserActivityType
{
    ProductCreated = 1,
    ProductUpdated = 2,
    InventoryAdded = 3,
    InventoryRemoved = 4,
    CashEntry = 5,
    CashExit = 6,
    CashFlowUpdated = 7,
    ShiftClosed = 8
}

public class UserActivityLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public Guid? CashBoxId { get; set; }
    public CashBox? CashBox { get; set; }
    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }
    public UserActivityType ActivityType { get; set; }
    public decimal? QuantityDelta { get; set; }
    public decimal? Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
