using Pos.Application.Dtos.CashBoxes;
using Pos.Application.Interfaces.Infrastructure;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashBoxes;

public class UpdateCashBoxUseCase
{
    private readonly ICashBoxRepository _cashBoxRepository;
    private readonly IUserActivityLogRepository _activityRepository;
    private readonly IShiftSummaryNotifier _shiftSummaryNotifier;
    private readonly ITransactionalExecutor _transactionalExecutor;

    public UpdateCashBoxUseCase(
        ICashBoxRepository cashBoxRepository,
        IUserActivityLogRepository activityRepository,
        IShiftSummaryNotifier shiftSummaryNotifier,
        ITransactionalExecutor transactionalExecutor)
    {
        _cashBoxRepository = cashBoxRepository;
        _activityRepository = activityRepository;
        _shiftSummaryNotifier = shiftSummaryNotifier;
        _transactionalExecutor = transactionalExecutor;
    }

    public async Task<CashBoxResponseDto> ExecuteAsync(CashBoxUpdateDto dto, Guid actorUserId)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "La caja no puede ser nula.");

        if (actorUserId == Guid.Empty)
            throw new ArgumentNullException(nameof(actorUserId), "El usuario es requerido.");

        var shouldNotifyShiftClosed = false;
        var updated = await _transactionalExecutor.ExecuteAsync(async () =>
        {
            var existing = await _cashBoxRepository.GetByIdAsync(dto.Id);
            shouldNotifyShiftClosed = existing.Status == Status.Open && dto.Status == Status.Closed;

            var cashBox = new CashBox
            {
                Id = dto.Id,
                UserId = existing.UserId,
                Status = dto.Status,
                OpeningBalance = dto.OpeningBalance,
                CashTotal = dto.CashTotal,
                CardTotal = dto.CardTotal,
                ExpectatedBalance = dto.ExpectatedBalance,
                ActualBalance = dto.ActualBalance,
                TicketsCount = dto.TicketsCount,
                UpdatedAt = DateTime.UtcNow
            };

            var current = await _cashBoxRepository.UpdateAsync(cashBox);

            if (shouldNotifyShiftClosed)
            {
                await _activityRepository.CreateAsync(new UserActivityLog
                {
                    UserId = actorUserId,
                    CashBoxId = current.Id,
                    ActivityType = UserActivityType.ShiftClosed,
                    Description = "Corte de turno cerrado.",
                    CreatedAt = DateTime.UtcNow
                });
            }

            return current;
        });

        if (shouldNotifyShiftClosed)
        {
            try
            {
                await _shiftSummaryNotifier.NotifyShiftClosedAsync(updated.Id, updated.UserId);
            }
            catch
            {
                // El cierre de turno no debe fallar por errores del canal de notificaciones.
            }
        }

        return Map(updated);
    }

    private static CashBoxResponseDto Map(CashBox cashBox)
    {
        return new CashBoxResponseDto
        {
            Id = cashBox.Id,
            UserId = cashBox.UserId,
            Status = cashBox.Status,
            CreatedAt = cashBox.CreatedAt,
            UpdatedAt = cashBox.UpdatedAt,
            OpeningBalance = cashBox.OpeningBalance,
            CashTotal = cashBox.CashTotal,
            CardTotal = cashBox.CardTotal,
            ExpectatedBalance = cashBox.ExpectatedBalance,
            ActualBalance = cashBox.ActualBalance,
            TicketsCount = cashBox.TicketsCount
        };
    }
}
