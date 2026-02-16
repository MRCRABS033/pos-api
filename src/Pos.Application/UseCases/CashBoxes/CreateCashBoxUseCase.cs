using Pos.Application.Dtos.CashBoxes;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashBoxes;

public class CreateCashBoxUseCase
{
    private readonly ICashBoxRepository _cashBoxRepository;

    public CreateCashBoxUseCase(ICashBoxRepository cashBoxRepository)
    {
        _cashBoxRepository = cashBoxRepository;
    }

    public async Task<CashBoxResponseDto> ExecuteAsync(CashBoxCreateDto dto, Guid actorUserId)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "La caja no puede ser nula.");

        if (actorUserId == Guid.Empty)
            throw new ArgumentNullException(nameof(actorUserId), "El usuario es requerido.");

        var now = DateTime.UtcNow;
        var cashBox = new CashBox
        {
            UserId = actorUserId,
            Status = dto.Status,
            CreatedAt = now,
            UpdatedAt = now,
            OpeningBalance = dto.OpeningBalance,
            CashTotal = dto.CashTotal,
            CardTotal = dto.CardTotal,
            ExpectatedBalance = dto.ExpectatedBalance,
            ActualBalance = dto.ActualBalance,
            TicketsCount = dto.TicketsCount
        };

        var created = await _cashBoxRepository.CreateAsync(cashBox);
        return Map(created);
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
