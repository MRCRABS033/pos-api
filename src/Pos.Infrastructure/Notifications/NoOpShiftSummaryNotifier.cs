using Pos.Application.Interfaces.Infrastructure;

namespace Pos.Infrastructure.Notifications;

public sealed class NoOpShiftSummaryNotifier : IShiftSummaryNotifier
{
    public Task NotifyShiftClosedAsync(Guid cashBoxId, Guid userId)
    {
        return Task.CompletedTask;
    }
}
