using Pos.Application.Dtos.Categories;
using Pos.Application.Dtos.Products;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Products;

public class UpdateProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly ICashBoxRepository _cashBoxRepository;
    private readonly IUserActivityLogRepository _activityRepository;

    public UpdateProductUseCase(
        IProductRepository productRepository,
        ICashBoxRepository cashBoxRepository,
        IUserActivityLogRepository activityRepository)
    {
        _productRepository = productRepository;
        _cashBoxRepository = cashBoxRepository;
        _activityRepository = activityRepository;
    }

    public async Task<ProductResponseDto> ExecuteAsync(ProductUpdateDto dto, Guid userId)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "El producto no puede ser nulo.");

        var current = await _productRepository.GetByIdAsync(dto.Id);

        var product = new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            Sku = dto.Sku,
            PriceCost = dto.PriceCost,
            PriceSell = dto.PriceSell,
            Stock = dto.Stock,
            IsActive = dto.IsActive,
            IsAvailable = dto.IsAvailable,
            CategoryId = dto.CategoryId,
            UpdatedAt = DateTime.UtcNow,
            UpdatedById = userId
        };

        var updated = await _productRepository.UpdateAsync(product);

        var latestCashBox = await _cashBoxRepository.GetLatestByUserId(userId);
        var cashBoxId = latestCashBox?.Status == Status.Open ? latestCashBox.Id : (Guid?)null;
        var stockDelta = updated.Stock - current.Stock;

        await _activityRepository.CreateAsync(new UserActivityLog
        {
            UserId = userId,
            CashBoxId = cashBoxId,
            ProductId = updated.Id,
            ActivityType = UserActivityType.ProductUpdated,
            Description = $"Producto actualizado: {updated.Name} ({updated.Sku})",
            CreatedAt = DateTime.UtcNow
        });

        if (stockDelta > 0)
        {
            await _activityRepository.CreateAsync(new UserActivityLog
            {
                UserId = userId,
                CashBoxId = cashBoxId,
                ProductId = updated.Id,
                ActivityType = UserActivityType.InventoryAdded,
                QuantityDelta = stockDelta,
                Description = $"Entrada de inventario para {updated.Name}",
                CreatedAt = DateTime.UtcNow
            });
        }
        else if (stockDelta < 0)
        {
            await _activityRepository.CreateAsync(new UserActivityLog
            {
                UserId = userId,
                CashBoxId = cashBoxId,
                ProductId = updated.Id,
                ActivityType = UserActivityType.InventoryRemoved,
                QuantityDelta = stockDelta,
                Description = $"Salida de inventario para {updated.Name}",
                CreatedAt = DateTime.UtcNow
            });
        }

        return Map(updated);
    }

    private static ProductResponseDto Map(Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Sku = product.Sku,
            PriceCost = product.PriceCost,
            PriceSell = product.PriceSell,
            Stock = product.Stock,
            IsActive = product.IsActive,
            IsAvailable = product.IsAvailable,
            CategoryId = product.CategoryId ?? Guid.Empty,
            Category = product.Category == null
                ? null
                : new CategoryResponseDto
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name
                },
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
}
