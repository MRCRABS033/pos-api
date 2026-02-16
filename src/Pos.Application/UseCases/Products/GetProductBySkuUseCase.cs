using Pos.Application.Dtos.Categories;
using Pos.Application.Dtos.Products;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Products;

public class GetProductBySkuUseCase
{
    private readonly IProductRepository _productRepository;
    
    public GetProductBySkuUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponseDto> ExecuteAsync(string sku)
    {
        var product = await _productRepository.GetByProductSku(sku);
        if (product is null)
            throw new KeyNotFoundException("Producto no encontrado.");

        return Map(product);
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
