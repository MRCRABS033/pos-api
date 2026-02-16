using Pos.Domain.Entities;

namespace Pos.Application.Dtos.CashBoxes;

public class ActivityLogItemDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? CashBoxId { get; set; }
    public Guid? ProductId { get; set; }
    public UserActivityType ActivityType { get; set; }
    public decimal? QuantityDelta { get; set; }
    public decimal? Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
