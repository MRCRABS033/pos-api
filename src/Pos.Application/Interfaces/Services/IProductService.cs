using Pos.Application.Dtos.Products;

namespace Pos.Application.Interfaces.Services;

public interface IProductService
{
    Task<ProductResponseDto> CreateAsync(ProductCreateDto dto, Guid userId);
    Task<ProductResponseDto> UpdateAsync(ProductUpdateDto dto, Guid userId);
    Task<ProductResponseDto> GetByIdAsync(Guid id);
    Task<ProductResponseDto> GetBySkuAsync(string sku);
    Task<IReadOnlyList<ProductResponseDto>> SearchAsync(string term);
    Task<IReadOnlyList<ProductResponseDto>> GetByCategoryIdAsync(Guid categoryId);
    Task<IReadOnlyList<ProductResponseDto>> GetAllAsync(int page = 1, int pageSize = 50);
    Task DeleteAsync(Guid id);
}
