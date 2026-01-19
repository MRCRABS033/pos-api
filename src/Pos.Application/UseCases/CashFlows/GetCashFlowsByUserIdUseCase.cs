using Pos.Application.Dtos.CashFlows;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashFlows;

public class GetCashFlowsByUserIdUseCase
{
    private readonly ICashFlowRepository _cashFlowRepository;

    public GetCashFlowsByUserIdUseCase(ICashFlowRepository cashFlowRepository)
    {
        _cashFlowRepository = cashFlowRepository;
    }

    public async Task<IReadOnlyList<CashFlowResponseDto>> ExecuteAsync(Guid userId)
    {
        var cashFlows = await _cashFlowRepository.GetByUserId(userId);
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
