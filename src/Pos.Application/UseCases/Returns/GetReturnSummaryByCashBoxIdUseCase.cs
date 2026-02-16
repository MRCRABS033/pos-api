using Pos.Application.Dtos.Returns;
using Pos.Domain.Interfaces.Repositories;

namespace Pos.Application.UseCases.Returns;

public class GetReturnSummaryByCashBoxIdUseCase
{
    private readonly IReturnRepository _returnRepository;

    public GetReturnSummaryByCashBoxIdUseCase(IReturnRepository returnRepository)
    {
        _returnRepository = returnRepository;
    }

    public async Task<ReturnSummaryDto> ExecuteAsync(Guid cashBoxId)
    {
        var summary = await _returnRepository.GetSummaryByCashBoxId(cashBoxId);
        return new ReturnSummaryDto
        {
            CashBoxId = cashBoxId,
            ReturnsCount = summary.Count,
            CashTotal = summary.CashTotal,
            CardTotal = summary.CardTotal,
            Total = summary.CashTotal + summary.CardTotal
        };
    }
}
