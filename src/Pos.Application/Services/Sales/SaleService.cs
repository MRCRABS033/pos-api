using Pos.Application.Dtos.Sales;
using Pos.Application.Interfaces.Services;
using Pos.Application.UseCases.Sales;

namespace Pos.Application.Services.Sales;

public class SaleService : ISaleService
{
    private readonly CreateSaleUseCase _create;
    private readonly UpdateSaleUseCase _update;
    private readonly GetSaleByIdUseCase _getById;
    private readonly GetSalesByUserIdUseCase _getByUserId;
    private readonly GetSalesByDateRangeUseCase _getByDateRange;
    private readonly GetAllSalesUseCase _getAll;
    private readonly DeleteSaleUseCase _delete;

    public SaleService(
        CreateSaleUseCase create,
        UpdateSaleUseCase update,
        GetSaleByIdUseCase getById,
        GetSalesByUserIdUseCase getByUserId,
        GetSalesByDateRangeUseCase getByDateRange,
        GetAllSalesUseCase getAll,
        DeleteSaleUseCase delete)
    {
        _create = create;
        _update = update;
        _getById = getById;
        _getByUserId = getByUserId;
        _getByDateRange = getByDateRange;
        _getAll = getAll;
        _delete = delete;
    }

    public Task<SaleResponseDto> CreateAsync(SaleCreateDto dto, Guid userId)
    {
        return _create.ExecuteAsync(dto, userId);
    }

    public Task<SaleResponseDto> UpdateAsync(SaleUpdateDto dto, Guid userId)
    {
        return _update.ExecuteAsync(dto, userId);
    }

    public Task<SaleResponseDto> GetByIdAsync(Guid id)
    {
        return _getById.ExecuteAsync(id);
    }

    public Task<IReadOnlyList<SaleResponseDto>> GetByUserIdAsync(Guid userId)
    {
        return _getByUserId.ExecuteAsync(userId);
    }

    public Task<IReadOnlyList<SaleResponseDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return _getByDateRange.ExecuteAsync(startDate, endDate);
    }

    public Task<IReadOnlyList<SaleResponseDto>> GetAllAsync()
    {
        return _getAll.ExecuteAsync();
    }

    public Task DeleteAsync(Guid id)
    {
        return _delete.ExecuteAsync(id);
    }
}
