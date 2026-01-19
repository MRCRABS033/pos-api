using Pos.Application.Dtos.Categories;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Categories;

public class UpdateCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryResponseDto> ExecuteAsync(CategoryUpdateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "La categoria no puede ser nula.");

        var category = new Category
        {
            Id = dto.Id,
            Name = dto.Name
        };

        var updated = await _categoryRepository.UpdateAsync(category);
        return Map(updated);
    }

    private static CategoryResponseDto Map(Category category)
    {
        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }
}
