using Pos.Application.Dtos.Categories;

namespace Pos.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto);
    Task<CategoryResponseDto> UpdateAsync(CategoryUpdateDto dto);
    Task<CategoryResponseDto> GetByIdAsync(Guid id);
    Task<CategoryResponseDto?> GetByNameAsync(string name);
    Task<IReadOnlyList<CategoryResponseDto>> GetAllAsync();
    Task DeleteAsync(Guid id);
}
