using Pos.Application.Dtos.Categories;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Categories;

public class GetAllCategoriesUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyList<CategoryResponseDto>> ExecuteAsync(int page = 1, int pageSize = 50)
    {
        var categories = await _categoryRepository.GetAllAsync(page, pageSize);
        return categories.Select(Map).ToList();
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
