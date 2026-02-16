using Pos.Application.Dtos.CashBoxes;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashBoxes;

public class GetShiftCutSummaryUseCase
{
    private readonly ICashBoxRepository _cashBoxRepository;
    private readonly IUserActivityLogRepository _activityRepository;
    private readonly IUserRepository _userRepository;

    public GetShiftCutSummaryUseCase(
        ICashBoxRepository cashBoxRepository,
        IUserActivityLogRepository activityRepository,
        IUserRepository userRepository)
    {
        _cashBoxRepository = cashBoxRepository;
        _activityRepository = activityRepository;
        _userRepository = userRepository;
    }

    public async Task<ShiftCutSummaryDto> ExecuteAsync(Guid cashBoxId)
    {
        var cashBox = await _cashBoxRepository.GetByIdAsync(cashBoxId);
        var activities = await _activityRepository.GetByCashBoxAsync(cashBoxId);

        var userGroups = activities
            .GroupBy(x => x.UserId)
            .ToList();

        var users = new List<ShiftCutUserSummaryDto>(userGroups.Count);
        foreach (var group in userGroups)
        {
            var user = await _userRepository.GetByIdAsync(group.Key);
            users.Add(new ShiftCutUserSummaryDto
            {
                UserId = user.Id,
                UserName = $"{user.Name} {user.LastName}".Trim(),
                TotalActivities = group.Count(),
                ProductsCreated = group.Count(x => x.ActivityType == UserActivityType.ProductCreated),
                ProductsUpdated = group.Count(x => x.ActivityType == UserActivityType.ProductUpdated),
                InventoryAddedTotal = group
                    .Where(x => x.ActivityType == UserActivityType.InventoryAdded)
                    .Sum(x => x.QuantityDelta ?? 0m),
                InventoryRemovedTotal = group
                    .Where(x => x.ActivityType == UserActivityType.InventoryRemoved)
                    .Sum(x => Math.Abs(x.QuantityDelta ?? 0m)),
                CashEntriesTotal = group
                    .Where(x => x.ActivityType == UserActivityType.CashEntry)
                    .Sum(x => x.Amount ?? 0m),
                CashExitsTotal = group
                    .Where(x => x.ActivityType == UserActivityType.CashExit)
                    .Sum(x => x.Amount ?? 0m)
            });
        }

        return new ShiftCutSummaryDto
        {
            CashBoxId = cashBox.Id,
            OwnerUserId = cashBox.UserId,
            Status = cashBox.Status,
            StartedAt = cashBox.CreatedAt,
            UpdatedAt = cashBox.UpdatedAt,
            OpeningBalance = cashBox.OpeningBalance,
            CashTotal = cashBox.CashTotal,
            CardTotal = cashBox.CardTotal,
            ExpectedBalance = cashBox.ExpectatedBalance,
            ActualBalance = cashBox.ActualBalance,
            TicketsCount = cashBox.TicketsCount,
            Users = users.OrderBy(u => u.UserName).ToList()
        };
    }
}
