using Pos.Application.Dtos.CashFlows;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashFlows;

public class GetAllCashFlowsUseCase
{
    private readonly ICashFlowRepository _cashFlowRepository;

    public GetAllCashFlowsUseCase(ICashFlowRepository cashFlowRepository)
    {
        _cashFlowRepository = cashFlowRepository;
    }

    public async Task<IReadOnlyList<CashFlowResponseDto>> ExecuteAsync(int page = 1, int pageSize = 50)
    {
        var cashFlows = await _cashFlowRepository.GetAllAsync(page, pageSize);
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
