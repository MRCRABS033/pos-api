using Pos.Application.Dtos.SaleItems;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.SaleItems;

public class GetAllSaleItemsUseCase
{
    private readonly ISaleItemRepository _saleItemRepository;

    public GetAllSaleItemsUseCase(ISaleItemRepository saleItemRepository)
    {
        _saleItemRepository = saleItemRepository;
    }

    public async Task<IReadOnlyList<SaleItemResponseDto>> ExecuteAsync(int page = 1, int pageSize = 50)
    {
        var saleItems = await _saleItemRepository.GetAllAsync(page, pageSize);
        return saleItems.Select(Map).ToList();
    }

    private static SaleItemResponseDto Map(SaleItem saleItem)
    {
        return new SaleItemResponseDto
        {
            Id = saleItem.Id,
            SaleId = saleItem.SaleId,
            ProductId = saleItem.ProductId,
            ProductName = saleItem.Product?.Name,
            Quantity = saleItem.Quantity,
            UnitPrice = saleItem.UnitPrice
        };
    }
}
