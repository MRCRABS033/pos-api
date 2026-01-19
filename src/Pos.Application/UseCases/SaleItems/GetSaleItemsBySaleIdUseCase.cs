using Pos.Application.Dtos.SaleItems;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.SaleItems;

public class GetSaleItemsBySaleIdUseCase
{
    private readonly ISaleItemRepository _saleItemRepository;

    public GetSaleItemsBySaleIdUseCase(ISaleItemRepository saleItemRepository)
    {
        _saleItemRepository = saleItemRepository;
    }

    public async Task<IReadOnlyList<SaleItemResponseDto>> ExecuteAsync(Guid saleId)
    {
        var saleItems = await _saleItemRepository.GetBySaleId(saleId);
        return saleItems.Select(Map).ToList();
    }

    private static SaleItemResponseDto Map(SaleItem saleItem)
    {
        return new SaleItemResponseDto
        {
            Id = saleItem.Id,
            SaleId = saleItem.SaleId,
            ProductId = saleItem.ProductId,
            Quantity = saleItem.Quantity,
            UnitPrice = saleItem.UnitPrice
        };
    }
}
