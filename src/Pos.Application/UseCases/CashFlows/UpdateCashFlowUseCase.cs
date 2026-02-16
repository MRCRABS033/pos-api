using Pos.Application.Dtos.CashFlows;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashFlows;

public class UpdateCashFlowUseCase
{
    private readonly ICashFlowRepository _cashFlowRepository;
    private readonly IUserActivityLogRepository _activityRepository;

    public UpdateCashFlowUseCase(
        ICashFlowRepository cashFlowRepository,
        IUserActivityLogRepository activityRepository)
    {
        _cashFlowRepository = cashFlowRepository;
        _activityRepository = activityRepository;
    }

    public async Task<CashFlowResponseDto> ExecuteAsync(CashFlowUpdateDto dto, Guid actorUserId)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "El flujo de caja no puede ser nulo.");

        if (actorUserId == Guid.Empty)
            throw new ArgumentNullException(nameof(actorUserId), "El usuario es requerido.");

        var cashFlow = new CashFlow
        {
            Id = dto.Id,
            Motive = dto.Motive,
            Amount = dto.Amount,
            Type = dto.Type,
            UserId = actorUserId,
            CashBoxId = dto.CashBoxId,
            UpdatedAt = DateTime.UtcNow
        };

        var updated = await _cashFlowRepository.UpdateAsync(cashFlow);

        await _activityRepository.CreateAsync(new UserActivityLog
        {
            UserId = actorUserId,
            CashBoxId = updated.CashBoxId,
            ActivityType = UserActivityType.CashFlowUpdated,
            Amount = updated.Amount,
            Description = $"Flujo actualizado: {updated.Motive}",
            CreatedAt = DateTime.UtcNow
        });

        return Map(updated);
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
