using Pos.Application.Dtos.CashFlows;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashFlows;

public class GetCashFlowsByDateRangeUseCase
{
    private readonly ICashFlowRepository _cashFlowRepository;

    public GetCashFlowsByDateRangeUseCase(ICashFlowRepository cashFlowRepository)
    {
        _cashFlowRepository = cashFlowRepository;
    }

    public async Task<IReadOnlyList<CashFlowResponseDto>> ExecuteAsync(DateTime startDate, DateTime endDate)
    {
        var cashFlows = await _cashFlowRepository.GetByDateRange(startDate, endDate);
        return cashFlows.Select(Map).ToList();
    }

    private static CashFlowResponseDto Map(CashFlow cashFlow)
    {
        return new CashFlowResponseDto
        {
            Id = cashFlow.Id,
            Motive = cashFlow.Motive,
            Amount = cashFlow.Amount,
            Type = cashFlow.Type,
            UserId = cashFlow.UserId,
            CashBoxId = cashFlow.CashBoxId,
            CreatedAt = cashFlow.CreatedAt,
            UpdatedAt = cashFlow.UpdatedAt
        };
    }
}
