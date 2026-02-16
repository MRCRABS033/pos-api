using Pos.Application.Dtos.Categories;
using Pos.Application.Dtos.Products;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Products;

public class GetProductsByCategoryUseCase
{
    private readonly IProductRepository _productRepository;

    public GetProductsByCategoryUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IReadOnlyList<ProductResponseDto>> ExecuteAsync(Guid categoryId)
    {
        var products = await _productRepository.GetByCategoryId(categoryId);
        return products.Select(Map).ToList();
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
