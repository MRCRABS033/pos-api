using Pos.Application.Dtos.Categories;
using Pos.Application.Interfaces.Services;
using Pos.Application.UseCases.Categories;

namespace Pos.Application.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly CreateCategoryUseCase _create;
    private readonly UpdateCategoryUseCase _update;
    private readonly GetCategoryByIdUseCase _getById;
    private readonly GetCategoryByNameUseCase _getByName;
    private readonly GetAllCategoriesUseCase _getAll;
    private readonly DeleteCategoryUseCase _delete;

    public CategoryService(
        CreateCategoryUseCase create,
        UpdateCategoryUseCase update,
        GetCategoryByIdUseCase getById,
        GetCategoryByNameUseCase getByName,
        GetAllCategoriesUseCase getAll,
        DeleteCategoryUseCase delete)
    {
        _create = create;
        _update = update;
        _getById = getById;
        _getByName = getByName;
        _getAll = getAll;
        _delete = delete;
    }

    public Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto)
    {
        return _create.ExecuteAsync(dto);
    }

    public Task<CategoryResponseDto> UpdateAsync(CategoryUpdateDto dto)
    {
        return _update.ExecuteAsync(dto);
    }

    public Task<CategoryResponseDto> GetByIdAsync(Guid id)
    {
        return _getById.ExecuteAsync(id);
    }

    public Task<CategoryResponseDto?> GetByNameAsync(string name)
    {
        return _getByName.ExecuteAsync(name);
    }

    public Task<IReadOnlyList<CategoryResponseDto>> GetAllAsync(int page = 1, int pageSize = 50)
    {
        return _getAll.ExecuteAsync(page, pageSize);
    }

    public Task DeleteAsync(Guid id)
    {
        return _delete.ExecuteAsync(id);
    }
}
