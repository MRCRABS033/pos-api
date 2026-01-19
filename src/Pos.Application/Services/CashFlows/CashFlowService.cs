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
    private readonly GetAllCashFlowsUseCase _getAll;
    private readonly DeleteCashFlowUseCase _delete;

    public CashFlowService(
        CreateCashFlowUseCase create,
        UpdateCashFlowUseCase update,
        GetCashFlowByIdUseCase getById,
        GetCashFlowsByUserIdUseCase getByUserId,
        GetCashFlowsByDateRangeUseCase getByDateRange,
        GetAllCashFlowsUseCase getAll,
        DeleteCashFlowUseCase delete)
    {
        _create = create;
        _update = update;
        _getById = getById;
        _getByUserId = getByUserId;
        _getByDateRange = getByDateRange;
        _getAll = getAll;
        _delete = delete;
    }

    public Task<CashFlowResponseDto> CreateAsync(CashFlowCreateDto dto)
    {
        return _create.ExecuteAsync(dto);
    }

    public Task<CashFlowResponseDto> UpdateAsync(CashFlowUpdateDto dto)
    {
        return _update.ExecuteAsync(dto);
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

    public Task<IReadOnlyList<CashFlowResponseDto>> GetAllAsync()
    {
        return _getAll.ExecuteAsync();
    }

    public Task DeleteAsync(Guid id)
    {
        return _delete.ExecuteAsync(id);
    }
}
