using Pos.Application.Dtos.Categories;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Categories;

public class GetCategoryByNameUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByNameUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryResponseDto?> ExecuteAsync(string name)
    {
        var category = await _categoryRepository.GetByName(name);
        return category == null ? null : Map(category);
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
