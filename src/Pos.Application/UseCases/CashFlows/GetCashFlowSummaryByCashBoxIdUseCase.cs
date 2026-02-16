using Pos.Application.Dtos.CashFlows;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashFlows;

public class GetCashFlowSummaryByCashBoxIdUseCase
{
    private readonly ICashFlowRepository _cashFlowRepository;

    public GetCashFlowSummaryByCashBoxIdUseCase(ICashFlowRepository cashFlowRepository)
    {
        _cashFlowRepository = cashFlowRepository;
    }

    public async Task<CashFlowSummaryDto> ExecuteAsync(Guid cashBoxId)
    {
        var summary = await _cashFlowRepository.GetSummaryByCashBoxId(cashBoxId);
        return new CashFlowSummaryDto
        {
            CashBoxId = cashBoxId,
            EntriesCount = summary.EntriesCount,
            ExitsCount = summary.ExitsCount,
            EntriesTotal = summary.EntriesTotal,
            ExitsTotal = summary.ExitsTotal,
            Total = summary.EntriesTotal - summary.ExitsTotal
        };
    }
}
