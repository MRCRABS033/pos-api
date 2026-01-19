using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.SaleItems;

public class DeleteSaleItemUseCase
{
    private readonly ISaleItemRepository _saleItemRepository;

    public DeleteSaleItemUseCase(ISaleItemRepository saleItemRepository)
    {
        _saleItemRepository = saleItemRepository;
    }

    public Task ExecuteAsync(Guid id)
    {
        return _saleItemRepository.DeleteAsync(id);
    }
}
