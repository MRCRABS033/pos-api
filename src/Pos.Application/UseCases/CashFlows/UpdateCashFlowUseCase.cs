using Pos.Application.Dtos.CashFlows;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashFlows;

public class UpdateCashFlowUseCase
{
    private readonly ICashFlowRepository _cashFlowRepository;

    public UpdateCashFlowUseCase(ICashFlowRepository cashFlowRepository)
    {
        _cashFlowRepository = cashFlowRepository;
    }

    public async Task<CashFlowResponseDto> ExecuteAsync(CashFlowUpdateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "El flujo de caja no puede ser nulo.");

        var cashFlow = new CashFlow
        {
            Id = dto.Id,
            Motive = dto.Motive,
            Amount = dto.Amount,
            UserId = dto.UserId,
            UpdatedAt = DateTime.UtcNow
        };

        var updated = await _cashFlowRepository.UpdateAsync(cashFlow);
        return Map(updated);
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
