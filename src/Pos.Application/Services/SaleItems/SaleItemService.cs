using Pos.Application.Dtos.SaleItems;
using Pos.Application.Interfaces.Services;
using Pos.Application.UseCases.SaleItems;

namespace Pos.Application.Services.SaleItems;

public class SaleItemService : ISaleItemService
{
    private readonly CreateSaleItemUseCase _create;
    private readonly UpdateSaleItemUseCase _update;
    private readonly GetSaleItemByIdUseCase _getById;
    private readonly GetSaleItemsBySaleIdUseCase _getBySaleId;
    private readonly GetSaleItemsByProductIdUseCase _getByProductId;
    private readonly GetAllSaleItemsUseCase _getAll;
    private readonly DeleteSaleItemUseCase _delete;

    public SaleItemService(
        CreateSaleItemUseCase create,
        UpdateSaleItemUseCase update,
        GetSaleItemByIdUseCase getById,
        GetSaleItemsBySaleIdUseCase getBySaleId,
        GetSaleItemsByProductIdUseCase getByProductId,
        GetAllSaleItemsUseCase getAll,
        DeleteSaleItemUseCase delete)
    {
        _create = create;
        _update = update;
        _getById = getById;
        _getBySaleId = getBySaleId;
        _getByProductId = getByProductId;
        _getAll = getAll;
        _delete = delete;
    }

    public Task<SaleItemResponseDto> CreateAsync(SaleItemCreateDto dto, Guid saleId)
    {
        return _create.ExecuteAsync(dto, saleId);
    }

    public Task<SaleItemResponseDto> UpdateAsync(SaleItemUpdateDto dto)
    {
        return _update.ExecuteAsync(dto);
    }

    public Task<SaleItemResponseDto> GetByIdAsync(Guid id)
    {
        return _getById.ExecuteAsync(id);
    }

    public Task<IReadOnlyList<SaleItemResponseDto>> GetBySaleIdAsync(Guid saleId)
    {
        return _getBySaleId.ExecuteAsync(saleId);
    }

    public Task<IReadOnlyList<SaleItemResponseDto>> GetByProductIdAsync(Guid productId)
    {
        return _getByProductId.ExecuteAsync(productId);
    }

    public Task<IReadOnlyList<SaleItemResponseDto>> GetAllAsync(int page = 1, int pageSize = 50)
    {
        return _getAll.ExecuteAsync(page, pageSize);
    }

    public Task DeleteAsync(Guid id)
    {
        return _delete.ExecuteAsync(id);
    }
}
