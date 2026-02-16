using Pos.Application.Dtos.Returns;
using Pos.Application.Interfaces.Infrastructure;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Returns;

public class CreateReturnUseCase
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICashBoxRepository _cashBoxRepository;
    private readonly IReturnRepository _returnRepository;
    private readonly ITransactionalExecutor _transactionalExecutor;

    public CreateReturnUseCase(
        ISaleRepository saleRepository,
        ISaleItemRepository saleItemRepository,
        IProductRepository productRepository,
        ICashBoxRepository cashBoxRepository,
        IReturnRepository returnRepository,
        ITransactionalExecutor transactionalExecutor)
    {
        _saleRepository = saleRepository;
        _saleItemRepository = saleItemRepository;
        _productRepository = productRepository;
        _cashBoxRepository = cashBoxRepository;
        _returnRepository = returnRepository;
        _transactionalExecutor = transactionalExecutor;
    }

    public async Task<ReturnResponseDto> ExecuteAsync(Guid saleId, ReturnCreateDto dto, Guid actorUserId)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "La devolución no puede ser nula.");

        if (dto.Items.Count == 0)
            throw new ArgumentException("Debe indicar al menos un item.", nameof(dto.Items));

        if (actorUserId == Guid.Empty)
            throw new ArgumentNullException(nameof(actorUserId), "El usuario es requerido.");

        return await _transactionalExecutor.ExecuteAsync(async () =>
        {
            var sale = await _saleRepository.GetByIdAsync(saleId);

            var saleItems = await _saleItemRepository.GetBySaleId(saleId);
            var saleItemById = saleItems.ToDictionary(i => i.Id, i => i);

            var requestedQuantities = dto.Items
                .GroupBy(i => i.SaleItemId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

            foreach (var item in dto.Items)
            {
                if (!saleItemById.ContainsKey(item.SaleItemId))
                    throw new KeyNotFoundException("El item no pertenece a la venta.");

                if (item.Quantity <= 0)
                    throw new ArgumentException("La cantidad a devolver debe ser mayor a cero.");
            }

            var saleItemIds = requestedQuantities.Keys.ToList();
            var returnedQuantities = await _returnRepository.GetReturnedQuantitiesBySaleItemIds(saleItemIds);

            foreach (var kvp in requestedQuantities)
            {
                var saleItem = saleItemById[kvp.Key];
                returnedQuantities.TryGetValue(kvp.Key, out var alreadyReturned);
                var maxReturnable = saleItem.Quantity - alreadyReturned;
                if (kvp.Value > maxReturnable)
                    throw new InvalidOperationException("No se puede devolver más de la cantidad vendida.");
            }

            var now = DateTime.UtcNow;
            var returnItems = new List<ReturnItem>();
            decimal total = 0;

            foreach (var item in dto.Items)
            {
                var saleItem = saleItemById[item.SaleItemId];
                total += item.Quantity * saleItem.UnitPrice;

                returnItems.Add(new ReturnItem
                {
                    SaleItemId = saleItem.Id,
                    ProductId = saleItem.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = saleItem.UnitPrice
                });
            }

            var newReturn = new Return
            {
                SaleId = sale.Id,
                UserId = actorUserId,
                CreatedAt = now,
                Reason = dto.Reason,
                Total = total,
                PaymentType = sale.PaymentType,
                Items = returnItems
            };

            var created = await _returnRepository.CreateAsync(newReturn);

            var requestedByProduct = returnItems
                .GroupBy(i => i.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

            foreach (var productReturn in requestedByProduct)
            {
                var product = await _productRepository.GetByIdAsync(productReturn.Key);
                product.Stock += productReturn.Value;
                product.UpdatedAt = DateTime.UtcNow;
                await _productRepository.UpdateAsync(product);
            }

            if (sale.CashBoxId.HasValue)
            {
                var cashBox = await _cashBoxRepository.GetByIdAsync(sale.CashBoxId.Value);
                if (cashBox.Status != Status.Open)
                    throw new InvalidOperationException("La caja está cerrada.");

                if (sale.PaymentType == PaymentType.Cash)
                    cashBox.CashTotal -= total;
                else
                    cashBox.CardTotal -= total;

                cashBox.ExpectatedBalance = cashBox.OpeningBalance + cashBox.CashTotal + cashBox.CardTotal;
                cashBox.UpdatedAt = DateTime.UtcNow;
                await _cashBoxRepository.UpdateAsync(cashBox);
            }

            return Map(created);
        });
    }

    private static ReturnResponseDto Map(Return ret)
    {
        return new ReturnResponseDto
        {
            Id = ret.Id,
            SaleId = ret.SaleId,
            UserId = ret.UserId,
            CreatedAt = ret.CreatedAt,
            Reason = ret.Reason,
            Total = ret.Total,
            PaymentType = ret.PaymentType,
            Items = ret.Items.Select(MapItem).ToList()
        };
    }

    private static ReturnItemResponseDto MapItem(ReturnItem item)
    {
        return new ReturnItemResponseDto
        {
            Id = item.Id,
            ReturnId = item.ReturnId,
            SaleItemId = item.SaleItemId,
            ProductId = item.ProductId,
            ProductName = item.Product?.Name,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice
        };
    }
}
