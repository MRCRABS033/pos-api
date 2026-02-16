using Pos.Application.Dtos.Categories;
using Pos.Application.Dtos.Products;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Products;

public class CreateProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly ICashBoxRepository _cashBoxRepository;
    private readonly IUserActivityLogRepository _activityRepository;

    public CreateProductUseCase(
        IProductRepository productRepository,
        ICashBoxRepository cashBoxRepository,
        IUserActivityLogRepository activityRepository)
    {
        _productRepository = productRepository;
        _cashBoxRepository = cashBoxRepository;
        _activityRepository = activityRepository;
    }

    public async Task<ProductResponseDto> ExecuteAsync(ProductCreateDto dto, Guid userId)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "El producto no puede ser nulo.");

        var now = DateTime.UtcNow;
        var product = new Product
        {
            Name = dto.Name,
            Sku = dto.Sku,
            PriceCost = dto.PriceCost,
            PriceSell = dto.PriceSell,
            Stock = dto.Stock,
            IsActive = dto.IsActive,
            IsAvailable = dto.IsAvailable,
            CategoryId = dto.CategoryId,
            CreatedAt = now,
            UpdatedAt = now,
            CreatedById = userId,
            UpdatedById = userId
        };

        var created = await _productRepository.CreateAsync(product);

        var latestCashBox = await _cashBoxRepository.GetLatestByUserId(userId);
        var cashBoxId = latestCashBox?.Status == Status.Open ? latestCashBox.Id : (Guid?)null;

        await _activityRepository.CreateAsync(new UserActivityLog
        {
            UserId = userId,
            CashBoxId = cashBoxId,
            ProductId = created.Id,
            ActivityType = UserActivityType.ProductCreated,
            Description = $"Producto creado: {created.Name} ({created.Sku})",
            CreatedAt = DateTime.UtcNow
        });

        if (created.Stock > 0)
        {
            await _activityRepository.CreateAsync(new UserActivityLog
            {
                UserId = userId,
                CashBoxId = cashBoxId,
                ProductId = created.Id,
                ActivityType = UserActivityType.InventoryAdded,
                QuantityDelta = created.Stock,
                Description = $"Inventario inicial para {created.Name}",
                CreatedAt = DateTime.UtcNow
            });
        }

        return Map(created);
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
