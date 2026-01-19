using Pos.Application.Dtos.SaleItems;
using Pos.Application.Dtos.Sales;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Sales;

public class GetSaleByIdUseCase
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleItemRepository _saleItemRepository;

    public GetSaleByIdUseCase(ISaleRepository saleRepository, ISaleItemRepository saleItemRepository)
    {
        _saleRepository = saleRepository;
        _saleItemRepository = saleItemRepository;
    }

    public async Task<SaleResponseDto> ExecuteAsync(Guid id)
    {
        var sale = await _saleRepository.GetByIdAsync(id);
        var items = await _saleItemRepository.GetBySaleId(id);

        return new SaleResponseDto
        {
            Id = sale.Id,
            UserId = sale.UserId,
            CreatedAt = sale.CreatedAt,
            UpdatedAt = sale.UpdatedAt,
            Items = items.Select(MapItem).ToList()
        };
    }

    private static SaleItemResponseDto MapItem(SaleItem saleItem)
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
