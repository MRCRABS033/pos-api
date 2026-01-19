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

    public async Task<IReadOnlyList<CashFlowResponseDto>> ExecuteAsync()
    {
        var cashFlows = await _cashFlowRepository.GetAllAsync();
        return cashFlows.Select(Map).ToList();
    }

    private static CashFlowResponseDto Map(CashFlow cashFlow)
    {
        return new CashFlowResponseDto
        {
            Id = cashFlow.Id,
            Motive = cashFlow.Motive,
            Amount = cashFlow.Amount,
            UserId = cashFlow.UserId,
            CreatedAt = cashFlow.CreatedAt,
            UpdatedAt = cashFlow.UpdatedAt
        };
    }
}
