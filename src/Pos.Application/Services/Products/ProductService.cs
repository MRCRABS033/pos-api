using Pos.Application.Dtos.Products;
using Pos.Application.Interfaces.Services;
using Pos.Application.UseCases.Products;

namespace Pos.Application.Services.Products;

public class ProductService : IProductService
{
    private readonly CreateProductUseCase _createProduct;
    private readonly UpdateProductUseCase _updateProduct;
    private readonly GetProductByIdUseCase _getProductById;
    private readonly GetProductBySkuUseCase _getProductBySku;
    private readonly SearchProductsUseCase _searchProducts;
    private readonly GetProductsByCategoryUseCase _getByCategory;
    private readonly GetAllProductsUseCase _getAll;
    private readonly DeleteProductUseCase _delete;

    public ProductService(
        CreateProductUseCase createProduct,
        UpdateProductUseCase updateProduct,
        GetProductByIdUseCase getProductById,
        GetProductBySkuUseCase getProductBySku,
        SearchProductsUseCase searchProducts,
        GetProductsByCategoryUseCase getByCategory,
        GetAllProductsUseCase getAll,
        DeleteProductUseCase delete)
    {
        _createProduct = createProduct;
        _updateProduct = updateProduct;
        _getProductById = getProductById;
        _getProductBySku = getProductBySku;
        _searchProducts = searchProducts;
        _getByCategory = getByCategory;
        _getAll = getAll;
        _delete = delete;
    }

    public Task<ProductResponseDto> CreateAsync(ProductCreateDto dto, Guid userId)
    {
        return _createProduct.ExecuteAsync(dto, userId);
    }

    public Task<ProductResponseDto> UpdateAsync(ProductUpdateDto dto, Guid userId)
    {
        return _updateProduct.ExecuteAsync(dto, userId);
    }

    public Task<ProductResponseDto> GetByIdAsync(Guid id)
    {
        return _getProductById.ExecuteAsync(id);
    }

    public Task<ProductResponseDto> GetBySkuAsync(string sku)
    {
        return _getProductBySku.ExecuteAsync(sku);
    }

    public Task<IReadOnlyList<ProductResponseDto>> SearchAsync(string term)
    {
        return _searchProducts.ExecuteAsync(term);
    }

    public Task<IReadOnlyList<ProductResponseDto>> GetByCategoryIdAsync(Guid categoryId)
    {
        return _getByCategory.ExecuteAsync(categoryId);
    }

    public Task<IReadOnlyList<ProductResponseDto>> GetAllAsync(int page = 1, int pageSize = 50)
    {
        return _getAll.ExecuteAsync(page, pageSize);
    }

    public Task DeleteAsync(Guid id)
    {
        return _delete.ExecuteAsync(id);
    }
}
