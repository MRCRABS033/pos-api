using Pos.Application.Dtos.CashBoxes;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashBoxes;

public class GetCashBoxesByUserIdUseCase
{
    private readonly ICashBoxRepository _cashBoxRepository;

    public GetCashBoxesByUserIdUseCase(ICashBoxRepository cashBoxRepository)
    {
        _cashBoxRepository = cashBoxRepository;
    }

    public async Task<IReadOnlyList<CashBoxResponseDto>> ExecuteAsync(Guid userId)
    {
        var cashBoxes = await _cashBoxRepository.GetAllByUserId(userId);
        return cashBoxes.Select(Map).ToList();
    }

    private static CashBoxResponseDto Map(CashBox cashBox)
    {
        return new CashBoxResponseDto
        {
            Id = cashBox.Id,
            UserId = cashBox.UserId,
            Status = cashBox.Status,
            CreatedAt = cashBox.CreatedAt,
            UpdatedAt = cashBox.UpdatedAt,
            OpeningBalance = cashBox.OpeningBalance,
            CashTotal = cashBox.CashTotal,
            CardTotal = cashBox.CardTotal,
            ExpectatedBalance = cashBox.ExpectatedBalance,
            ActualBalance = cashBox.ActualBalance,
            TicketsCount = cashBox.TicketsCount
        };
    }
}
