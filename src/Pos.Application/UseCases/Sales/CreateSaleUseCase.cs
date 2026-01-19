using Pos.Application.Dtos.SaleItems;
using Pos.Application.Dtos.Sales;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Sales;

public class CreateSaleUseCase
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleItemRepository _saleItemRepository;

    public CreateSaleUseCase(ISaleRepository saleRepository, ISaleItemRepository saleItemRepository)
    {
        _saleRepository = saleRepository;
        _saleItemRepository = saleItemRepository;
    }

    public async Task<SaleResponseDto> ExecuteAsync(SaleCreateDto dto, Guid userId)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "La venta no puede ser nula.");

        var now = DateTime.UtcNow;
        var sale = new Sale
        {
            UserId = userId,
            CreatedAt = now,
            UpdatedAt = now
        };

        var created = await _saleRepository.CreateAsync(sale);

        var items = new List<SaleItemResponseDto>();
        foreach (var item in dto.Items)
        {
            var saleItem = new SaleItem
            {
                SaleId = created.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            };

            var createdItem = await _saleItemRepository.CreateAsync(saleItem);
            items.Add(MapItem(createdItem));
        }

        return new SaleResponseDto
        {
            Id = created.Id,
            UserId = created.UserId,
            CreatedAt = created.CreatedAt,
            UpdatedAt = created.UpdatedAt,
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
