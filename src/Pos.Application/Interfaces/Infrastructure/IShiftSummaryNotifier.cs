namespace Pos.Application.Interfaces.Infrastructure;

public interface IShiftSummaryNotifier
{
    Task NotifyShiftClosedAsync(Guid cashBoxId, Guid userId);
}
