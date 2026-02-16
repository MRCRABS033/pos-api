using Pos.Application.Dtos.CashBoxes;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashBoxes;

public class GetAllCashBoxesUseCase
{
    private readonly ICashBoxRepository _cashBoxRepository;

    public GetAllCashBoxesUseCase(ICashBoxRepository cashBoxRepository)
    {
        _cashBoxRepository = cashBoxRepository;
    }

    public async Task<IReadOnlyList<CashBoxResponseDto>> ExecuteAsync(int page = 1, int pageSize = 50)
    {
        var cashBoxes = await _cashBoxRepository.GetAllAsync(page, pageSize);
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
