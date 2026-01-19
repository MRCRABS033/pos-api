using Pos.Application.Dtos.SaleItems;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.SaleItems;

public class UpdateSaleItemUseCase
{
    private readonly ISaleItemRepository _saleItemRepository;

    public UpdateSaleItemUseCase(ISaleItemRepository saleItemRepository)
    {
        _saleItemRepository = saleItemRepository;
    }

    public async Task<SaleItemResponseDto> ExecuteAsync(SaleItemUpdateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "El item de venta no puede ser nulo.");

        var saleItem = new SaleItem
        {
            Id = dto.Id,
            SaleId = dto.SaleId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice
        };

        var updated = await _saleItemRepository.UpdateAsync(saleItem);
        return Map(updated);
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
