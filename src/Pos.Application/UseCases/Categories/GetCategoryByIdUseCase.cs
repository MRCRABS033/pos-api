using Pos.Application.Dtos.Categories;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Categories;

public class GetCategoryByIdUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryResponseDto> ExecuteAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return Map(category);
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
