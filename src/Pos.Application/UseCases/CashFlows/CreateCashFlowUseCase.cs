using Pos.Application.Dtos.CashFlows;
using Pos.Application.Interfaces.Infrastructure;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashFlows;

public class CreateCashFlowUseCase
{
    private readonly ICashFlowRepository _cashFlowRepository;
    private readonly ICashBoxRepository _cashBoxRepository;
    private readonly IUserActivityLogRepository _activityRepository;
    private readonly ITransactionalExecutor _transactionalExecutor;

    public CreateCashFlowUseCase(
        ICashFlowRepository cashFlowRepository,
        ICashBoxRepository cashBoxRepository,
        IUserActivityLogRepository activityRepository,
        ITransactionalExecutor transactionalExecutor)
    {
        _cashFlowRepository = cashFlowRepository;
        _cashBoxRepository = cashBoxRepository;
        _activityRepository = activityRepository;
        _transactionalExecutor = transactionalExecutor;
    }

    public async Task<CashFlowResponseDto> ExecuteAsync(CashFlowCreateDto dto, Guid actorUserId)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "El flujo de caja no puede ser nulo.");

        if (dto.CashBoxId == Guid.Empty)
            throw new ArgumentNullException(nameof(dto.CashBoxId), "La caja es requerida.");

        if (dto.Amount <= 0)
            throw new ArgumentException("El monto debe ser mayor a cero.", nameof(dto.Amount));

        if (actorUserId == Guid.Empty)
            throw new ArgumentNullException(nameof(actorUserId), "El usuario es requerido.");

        return await _transactionalExecutor.ExecuteAsync(async () =>
        {
            var cashBox = await _cashBoxRepository.GetByIdAsync(dto.CashBoxId);
            if (cashBox.Status != Status.Open)
                throw new InvalidOperationException("La caja está cerrada.");

            if (dto.Type == CashFlowType.Exit && dto.Amount > cashBox.CashTotal)
                throw new InvalidOperationException("Fondos insuficientes en caja.");

            var now = DateTime.UtcNow;
            var cashFlow = new CashFlow
            {
                Motive = dto.Motive,
                Amount = dto.Amount,
                Type = dto.Type,
                UserId = actorUserId,
                CashBoxId = dto.CashBoxId,
                CreatedAt = now,
                UpdatedAt = now
            };

            var created = await _cashFlowRepository.CreateAsync(cashFlow);

            await _activityRepository.CreateAsync(new UserActivityLog
            {
                UserId = actorUserId,
                CashBoxId = dto.CashBoxId,
                ActivityType = dto.Type == CashFlowType.Entry
                    ? UserActivityType.CashEntry
                    : UserActivityType.CashExit,
                Amount = dto.Amount,
                Description = dto.Motive,
                CreatedAt = DateTime.UtcNow
            });

            if (dto.Type == CashFlowType.Exit)
                cashBox.CashTotal -= dto.Amount;
            else
                cashBox.CashTotal += dto.Amount;
            cashBox.ExpectatedBalance = cashBox.OpeningBalance + cashBox.CashTotal + cashBox.CardTotal;
            cashBox.UpdatedAt = DateTime.UtcNow;
            await _cashBoxRepository.UpdateAsync(cashBox);

            return Map(created);
        });
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
