using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Sales;

public class DeleteSaleUseCase
{
    private readonly ISaleRepository _saleRepository;

    public DeleteSaleUseCase(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public Task ExecuteAsync(Guid id)
    {
        return _saleRepository.DeleteAsync(id);
    }
}
