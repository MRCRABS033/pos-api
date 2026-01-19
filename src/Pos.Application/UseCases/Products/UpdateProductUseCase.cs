using Pos.Application.Dtos.Products;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Products;

public class UpdateProductUseCase
{
    private readonly IProductRepository _productRepository;

    public UpdateProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponseDto> ExecuteAsync(ProductUpdateDto dto, Guid userId)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "El producto no puede ser nulo.");

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
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
}
