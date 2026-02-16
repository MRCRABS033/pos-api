using Pos.Application.Dtos.SaleItems;
using Pos.Application.Dtos.Sales;
using Pos.Application.Interfaces.Infrastructure;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Sales;

public class CreateSaleUseCase
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICashBoxRepository _cashBoxRepository;
    private readonly ITransactionalExecutor _transactionalExecutor;

    public CreateSaleUseCase(
        ISaleRepository saleRepository,
        ISaleItemRepository saleItemRepository,
        IProductRepository productRepository,
        ICashBoxRepository cashBoxRepository,
        ITransactionalExecutor transactionalExecutor)
    {
        _saleRepository = saleRepository;
        _saleItemRepository = saleItemRepository;
        _productRepository = productRepository;
        _cashBoxRepository = cashBoxRepository;
        _transactionalExecutor = transactionalExecutor;
    }

    public async Task<SaleResponseDto> ExecuteAsync(SaleCreateDto dto, Guid userId)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "La venta no puede ser nula.");

        if (dto.CashBoxId == Guid.Empty)
            throw new ArgumentNullException(nameof(dto.CashBoxId), "La caja no puede ser nula.");

        return await _transactionalExecutor.ExecuteAsync(async () =>
        {
            var cashBox = await _cashBoxRepository.GetByIdAsync(dto.CashBoxId);
            if (cashBox.Status != Status.Open)
                throw new InvalidOperationException("La caja está cerrada.");

            var now = DateTime.UtcNow;
            var total = dto.Items.Sum(i => i.Quantity * i.UnitPrice);
            var discount = dto.Discount < 0 ? 0 : dto.Discount;
            var netTotal = Math.Max(0, total - discount);
            var sale = new Sale
            {
                UserId = userId,
                CashBoxId = dto.CashBoxId,
                PaymentType = dto.PaymentType,
                Discount = discount,
                Total = netTotal,
                CreatedAt = now,
                UpdatedAt = now
            };

            var created = await _saleRepository.CreateAsync(sale);

            var requestedByProduct = dto.Items
                .GroupBy(i => i.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

            var productsToUpdate = new List<Product>(requestedByProduct.Count);
            foreach (var requested in requestedByProduct)
            {
                var product = await _productRepository.GetByIdAsync(requested.Key);
                if (product.Stock < requested.Value)
                    throw new InvalidOperationException("Stock insuficiente para el producto.");

                product.Stock -= requested.Value;
                product.UpdatedById = userId;
                product.UpdatedAt = DateTime.UtcNow;
                productsToUpdate.Add(product);
            }

            foreach (var product in productsToUpdate)
            {
                await _productRepository.UpdateAsync(product);
            }

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

            if (created.PaymentType == PaymentType.Cash)
                cashBox.CashTotal += netTotal;
            else
                cashBox.CardTotal += netTotal;

            cashBox.TicketsCount += 1;
            cashBox.ExpectatedBalance = cashBox.OpeningBalance + cashBox.CashTotal + cashBox.CardTotal;
            cashBox.UpdatedAt = DateTime.UtcNow;
            await _cashBoxRepository.UpdateAsync(cashBox);

            return new SaleResponseDto
            {
                Id = created.Id,
                UserId = created.UserId,
                CashBoxId = created.CashBoxId,
                PaymentType = created.PaymentType,
                Total = created.Total,
                Discount = created.Discount,
                ItemsCount = items.Count,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt,
                Items = items
            };
        });
    }

    private static SaleItemResponseDto MapItem(SaleItem saleItem)
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
