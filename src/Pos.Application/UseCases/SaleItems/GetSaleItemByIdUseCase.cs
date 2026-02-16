using Pos.Application.Dtos.SaleItems;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.SaleItems;

public class GetSaleItemByIdUseCase
{
    private readonly ISaleItemRepository _saleItemRepository;

    public GetSaleItemByIdUseCase(ISaleItemRepository saleItemRepository)
    {
        _saleItemRepository = saleItemRepository;
    }

    public async Task<SaleItemResponseDto> ExecuteAsync(Guid id)
    {
        var saleItem = await _saleItemRepository.GetByIdAsync(id);
        return Map(saleItem);
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
