using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Products;

public class DeleteProductUseCase
{
    private readonly IProductRepository _productRepository;

    public DeleteProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task ExecuteAsync(Guid id)
    {
        return _productRepository.DeleteAsync(id);
    }
}
