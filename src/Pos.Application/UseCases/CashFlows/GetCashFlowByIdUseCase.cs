using Pos.Application.Dtos.CashFlows;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashFlows;

public class GetCashFlowByIdUseCase
{
    private readonly ICashFlowRepository _cashFlowRepository;

    public GetCashFlowByIdUseCase(ICashFlowRepository cashFlowRepository)
    {
        _cashFlowRepository = cashFlowRepository;
    }

    public async Task<CashFlowResponseDto> ExecuteAsync(Guid id)
    {
        var cashFlow = await _cashFlowRepository.GetByIdAsync(id);
        return Map(cashFlow);
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
