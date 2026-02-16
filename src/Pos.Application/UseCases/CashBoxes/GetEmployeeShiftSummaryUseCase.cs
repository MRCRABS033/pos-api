using Pos.Application.Dtos.CashBoxes;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashBoxes;

public class GetEmployeeShiftSummaryUseCase
{
    private readonly ICashBoxRepository _cashBoxRepository;
    private readonly IUserActivityLogRepository _activityRepository;

    public GetEmployeeShiftSummaryUseCase(
        ICashBoxRepository cashBoxRepository,
        IUserActivityLogRepository activityRepository)
    {
        _cashBoxRepository = cashBoxRepository;
        _activityRepository = activityRepository;
    }

    public async Task<EmployeeShiftSummaryDto> ExecuteAsync(Guid cashBoxId, Guid userId)
    {
        var cashBox = await _cashBoxRepository.GetByIdAsync(cashBoxId);
        var activities = await _activityRepository.GetByCashBoxAndUserAsync(cashBox.Id, userId);

        return new EmployeeShiftSummaryDto
        {
            CashBoxId = cashBox.Id,
            UserId = userId,
            TotalActivities = activities.Count,
            ProductsCreated = activities.Count(x => x.ActivityType == UserActivityType.ProductCreated),
            ProductsUpdated = activities.Count(x => x.ActivityType == UserActivityType.ProductUpdated),
            InventoryAddedTotal = activities
                .Where(x => x.ActivityType == UserActivityType.InventoryAdded)
                .Sum(x => x.QuantityDelta ?? 0m),
            InventoryRemovedTotal = activities
                .Where(x => x.ActivityType == UserActivityType.InventoryRemoved)
                .Sum(x => Math.Abs(x.QuantityDelta ?? 0m)),
            CashEntriesCount = activities.Count(x => x.ActivityType == UserActivityType.CashEntry),
            CashExitsCount = activities.Count(x => x.ActivityType == UserActivityType.CashExit),
            CashEntriesTotal = activities
                .Where(x => x.ActivityType == UserActivityType.CashEntry)
                .Sum(x => x.Amount ?? 0m),
            CashExitsTotal = activities
                .Where(x => x.ActivityType == UserActivityType.CashExit)
                .Sum(x => x.Amount ?? 0m),
            Activities = activities
                .OrderByDescending(x => x.CreatedAt)
                .Select(MapItem)
                .ToList()
        };
    }

    private static ActivityLogItemDto MapItem(UserActivityLog activity)
    {
        return new ActivityLogItemDto
        {
            Id = activity.Id,
            UserId = activity.UserId,
            CashBoxId = activity.CashBoxId,
            ProductId = activity.ProductId,
            ActivityType = activity.ActivityType,
            QuantityDelta = activity.QuantityDelta,
            Amount = activity.Amount,
            Description = activity.Description,
            CreatedAt = activity.CreatedAt
        };
    }
}
