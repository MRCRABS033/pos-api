using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashBoxes;

public class DeleteCashBoxUseCase
{
    private readonly ICashBoxRepository _cashBoxRepository;

    public DeleteCashBoxUseCase(ICashBoxRepository cashBoxRepository)
    {
        _cashBoxRepository = cashBoxRepository;
    }

    public Task ExecuteAsync(Guid id)
    {
        return _cashBoxRepository.DeleteAsync(id);
    }
}
