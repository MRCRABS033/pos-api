using Pos.Application.Dtos.CashBoxes;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashBoxes;

public class GetLatestCashBoxByUserIdUseCase
{
    private readonly ICashBoxRepository _cashBoxRepository;

    public GetLatestCashBoxByUserIdUseCase(ICashBoxRepository cashBoxRepository)
    {
        _cashBoxRepository = cashBoxRepository;
    }

    public async Task<CashBoxResponseDto> ExecuteAsync(Guid userId)
    {
        var cashBox = await _cashBoxRepository.GetLatestByUserId(userId);
        if (cashBox is null)
            throw new KeyNotFoundException("No se encontró una caja para el usuario.");

        return Map(cashBox);
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
