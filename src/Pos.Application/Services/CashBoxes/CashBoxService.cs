using Pos.Application.Dtos.CashBoxes;
using Pos.Application.Interfaces.Services;
using Pos.Application.UseCases.CashBoxes;

namespace Pos.Application.Services.CashBoxes;

public class CashBoxService : ICashBoxService
{
    private readonly CreateCashBoxUseCase _create;
    private readonly UpdateCashBoxUseCase _update;
    private readonly GetCashBoxByIdUseCase _getById;
    private readonly GetCashBoxByDateUseCase _getByDate;
    private readonly GetLatestCashBoxByUserIdUseCase _getLatestByUserId;
    private readonly GetCashBoxesByUserIdUseCase _getByUserId;
    private readonly GetAllCashBoxesUseCase _getAll;
    private readonly GetEmployeeShiftSummaryUseCase _getEmployeeShiftSummary;
    private readonly GetShiftCutSummaryUseCase _getShiftCutSummary;
    private readonly DeleteCashBoxUseCase _delete;

    public CashBoxService(
        CreateCashBoxUseCase create,
        UpdateCashBoxUseCase update,
        GetCashBoxByIdUseCase getById,
        GetCashBoxByDateUseCase getByDate,
        GetLatestCashBoxByUserIdUseCase getLatestByUserId,
        GetCashBoxesByUserIdUseCase getByUserId,
        GetAllCashBoxesUseCase getAll,
        GetEmployeeShiftSummaryUseCase getEmployeeShiftSummary,
        GetShiftCutSummaryUseCase getShiftCutSummary,
        DeleteCashBoxUseCase delete)
    {
        _create = create;
        _update = update;
        _getById = getById;
        _getByDate = getByDate;
        _getLatestByUserId = getLatestByUserId;
        _getByUserId = getByUserId;
        _getAll = getAll;
        _getEmployeeShiftSummary = getEmployeeShiftSummary;
        _getShiftCutSummary = getShiftCutSummary;
        _delete = delete;
    }

    public Task<CashBoxResponseDto> CreateAsync(CashBoxCreateDto dto, Guid actorUserId)
    {
        return _create.ExecuteAsync(dto, actorUserId);
    }

    public Task<CashBoxResponseDto> UpdateAsync(CashBoxUpdateDto dto, Guid actorUserId)
    {
        return _update.ExecuteAsync(dto, actorUserId);
    }

    public Task<CashBoxResponseDto> GetByIdAsync(Guid id)
    {
        return _getById.ExecuteAsync(id);
    }

    public Task<IReadOnlyList<CashBoxResponseDto>> GetByDateAsync(DateTime date)
    {
        return _getByDate.ExecuteAsync(date);
    }

    public Task<CashBoxResponseDto> GetLatestByUserIdAsync(Guid userId)
    {
        return _getLatestByUserId.ExecuteAsync(userId);
    }

    public Task<IReadOnlyList<CashBoxResponseDto>> GetByUserIdAsync(Guid userId)
    {
        return _getByUserId.ExecuteAsync(userId);
    }

    public Task<IReadOnlyList<CashBoxResponseDto>> GetAllAsync(int page = 1, int pageSize = 50)
    {
        return _getAll.ExecuteAsync(page, pageSize);
    }

    public Task<EmployeeShiftSummaryDto> GetEmployeeShiftSummaryAsync(Guid cashBoxId, Guid userId)
    {
        return _getEmployeeShiftSummary.ExecuteAsync(cashBoxId, userId);
    }

    public Task<ShiftCutSummaryDto> GetShiftCutSummaryAsync(Guid cashBoxId)
    {
        return _getShiftCutSummary.ExecuteAsync(cashBoxId);
    }

    public Task DeleteAsync(Guid id)
    {
        return _delete.ExecuteAsync(id);
    }
}
