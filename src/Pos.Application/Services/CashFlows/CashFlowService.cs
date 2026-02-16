using Pos.Application.Dtos.CashFlows;
using Pos.Application.Interfaces.Services;
using Pos.Application.UseCases.CashFlows;

namespace Pos.Application.Services.CashFlows;

public class CashFlowService : ICashFlowService
{
    private readonly CreateCashFlowUseCase _create;
    private readonly UpdateCashFlowUseCase _update;
    private readonly GetCashFlowByIdUseCase _getById;
    private readonly GetCashFlowsByUserIdUseCase _getByUserId;
    private readonly GetCashFlowsByDateRangeUseCase _getByDateRange;
    private readonly GetCashFlowsByCashBoxIdUseCase _getByCashBoxId;
    private readonly GetCashFlowSummaryByCashBoxIdUseCase _getSummaryByCashBoxId;
    private readonly GetAllCashFlowsUseCase _getAll;
    private readonly DeleteCashFlowUseCase _delete;

    public CashFlowService(
        CreateCashFlowUseCase create,
        UpdateCashFlowUseCase update,
        GetCashFlowByIdUseCase getById,
        GetCashFlowsByUserIdUseCase getByUserId,
        GetCashFlowsByDateRangeUseCase getByDateRange,
        GetCashFlowsByCashBoxIdUseCase getByCashBoxId,
        GetCashFlowSummaryByCashBoxIdUseCase getSummaryByCashBoxId,
        GetAllCashFlowsUseCase getAll,
        DeleteCashFlowUseCase delete)
    {
        _create = create;
        _update = update;
        _getById = getById;
        _getByUserId = getByUserId;
        _getByDateRange = getByDateRange;
        _getByCashBoxId = getByCashBoxId;
        _getSummaryByCashBoxId = getSummaryByCashBoxId;
        _getAll = getAll;
        _delete = delete;
    }

    public Task<CashFlowResponseDto> CreateAsync(CashFlowCreateDto dto, Guid actorUserId)
    {
        return _create.ExecuteAsync(dto, actorUserId);
    }

    public Task<CashFlowResponseDto> UpdateAsync(CashFlowUpdateDto dto, Guid actorUserId)
    {
        return _update.ExecuteAsync(dto, actorUserId);
    }

    public Task<CashFlowResponseDto> GetByIdAsync(Guid id)
    {
        return _getById.ExecuteAsync(id);
    }

    public Task<IReadOnlyList<CashFlowResponseDto>> GetByUserIdAsync(Guid userId)
    {
        return _getByUserId.ExecuteAsync(userId);
    }

    public Task<IReadOnlyList<CashFlowResponseDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return _getByDateRange.ExecuteAsync(startDate, endDate);
    }

    public Task<IReadOnlyList<CashFlowResponseDto>> GetByCashBoxIdAsync(Guid cashBoxId)
    {
        return _getByCashBoxId.ExecuteAsync(cashBoxId);
    }

    public Task<CashFlowSummaryDto> GetSummaryByCashBoxIdAsync(Guid cashBoxId)
    {
        return _getSummaryByCashBoxId.ExecuteAsync(cashBoxId);
    }

    public Task<IReadOnlyList<CashFlowResponseDto>> GetAllAsync(int page = 1, int pageSize = 50)
    {
        return _getAll.ExecuteAsync(page, pageSize);
    }

    public Task DeleteAsync(Guid id)
    {
        return _delete.ExecuteAsync(id);
    }
}
