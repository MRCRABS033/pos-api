using Pos.Application.Dtos.Products;

namespace Pos.Application.Interfaces.Services;

public interface IProductService
{
    Task<ProductResponseDto> CreateAsync(ProductCreateDto dto, Guid userId);
    Task<ProductResponseDto> UpdateAsync(ProductUpdateDto dto, Guid userId);
    Task<ProductResponseDto> GetByIdAsync(Guid id);
    Task<IReadOnlyList<ProductResponseDto>> SearchAsync(string term);
    Task<IReadOnlyList<ProductResponseDto>> GetByCategoryIdAsync(Guid categoryId);
    Task<IReadOnlyList<ProductResponseDto>> GetAllAsync();
    Task DeleteAsync(Guid id);
}
