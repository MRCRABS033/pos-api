using Pos.Application.Dtos.CashFlows;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashFlows;

public class CreateCashFlowUseCase
{
    private readonly ICashFlowRepository _cashFlowRepository;

    public CreateCashFlowUseCase(ICashFlowRepository cashFlowRepository)
    {
        _cashFlowRepository = cashFlowRepository;
    }

    public async Task<CashFlowResponseDto> ExecuteAsync(CashFlowCreateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "El flujo de caja no puede ser nulo.");

        var now = DateTime.UtcNow;
        var cashFlow = new CashFlow
        {
            Motive = dto.Motive,
            Amount = dto.Amount,
            UserId = dto.UserId,
            CreatedAt = now,
            UpdatedAt = now
        };

        var created = await _cashFlowRepository.CreateAsync(cashFlow);
        return Map(created);
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
