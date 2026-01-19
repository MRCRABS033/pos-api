using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.CashFlows;

public class DeleteCashFlowUseCase
{
    private readonly ICashFlowRepository _cashFlowRepository;

    public DeleteCashFlowUseCase(ICashFlowRepository cashFlowRepository)
    {
        _cashFlowRepository = cashFlowRepository;
    }

    public Task ExecuteAsync(Guid id)
    {
        return _cashFlowRepository.DeleteAsync(id);
    }
}
