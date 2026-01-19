using Pos.Application.Dtos.Categories;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Categories;

public class CreateCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryResponseDto> ExecuteAsync(CategoryCreateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "La categoria no puede ser nula.");

        var category = new Category
        {
            Name = dto.Name
        };

        var created = await _categoryRepository.CreateAsync(category);
        return Map(created);
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
