using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Categories;

public class DeleteCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task ExecuteAsync(Guid id)
    {
        return _categoryRepository.DeleteAsync(id);
    }
}
