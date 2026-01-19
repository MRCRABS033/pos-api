using Pos.Application.Dtos.SaleItems;
using Pos.Application.Dtos.Sales;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Sales;

public class UpdateSaleUseCase
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleItemRepository _saleItemRepository;

    public UpdateSaleUseCase(ISaleRepository saleRepository, ISaleItemRepository saleItemRepository)
    {
        _saleRepository = saleRepository;
        _saleItemRepository = saleItemRepository;
    }

    public async Task<SaleResponseDto> ExecuteAsync(SaleUpdateDto dto, Guid userId)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "La venta no puede ser nula.");

        var sale = new Sale
        {
            Id = dto.Id,
            UserId = userId,
            UpdatedAt = DateTime.UtcNow
        };

        var updated = await _saleRepository.UpdateAsync(sale);

        var items = new List<SaleItemResponseDto>();
        foreach (var item in dto.Items)
        {
            var saleItem = new SaleItem
            {
                Id = item.Id,
                SaleId = item.SaleId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            };

            var updatedItem = await _saleItemRepository.UpdateAsync(saleItem);
            items.Add(MapItem(updatedItem));
        }

        return new SaleResponseDto
        {
            Id = updated.Id,
            UserId = updated.UserId,
            CreatedAt = updated.CreatedAt,
            UpdatedAt = updated.UpdatedAt,
            Items = items
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
