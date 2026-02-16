using Pos.Application.Dtos.SaleItems;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.SaleItems;

public class GetSaleItemsByProductIdUseCase
{
    private readonly ISaleItemRepository _saleItemRepository;

    public GetSaleItemsByProductIdUseCase(ISaleItemRepository saleItemRepository)
    {
        _saleItemRepository = saleItemRepository;
    }

    public async Task<IReadOnlyList<SaleItemResponseDto>> ExecuteAsync(Guid productId)
    {
        var saleItems = await _saleItemRepository.GetByProductId(productId);
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
