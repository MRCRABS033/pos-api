using Pos.Application.Dtos.Returns;
using Pos.Application.Interfaces.Services;
using Pos.Application.UseCases.Returns;

namespace Pos.Application.Services.Returns;

public class ReturnService : IReturnService
{
    private readonly CreateReturnUseCase _create;
    private readonly GetReturnsBySaleIdUseCase _getBySaleId;
    private readonly GetReturnsByCashBoxIdUseCase _getByCashBoxId;
    private readonly GetReturnSummaryByCashBoxIdUseCase _getSummaryByCashBoxId;
    private readonly GetReturnByIdUseCase _getById;

    public ReturnService(
        CreateReturnUseCase create,
        GetReturnsBySaleIdUseCase getBySaleId,
        GetReturnsByCashBoxIdUseCase getByCashBoxId,
        GetReturnSummaryByCashBoxIdUseCase getSummaryByCashBoxId,
        GetReturnByIdUseCase getById)
    {
        _create = create;
        _getBySaleId = getBySaleId;
        _getByCashBoxId = getByCashBoxId;
        _getSummaryByCashBoxId = getSummaryByCashBoxId;
        _getById = getById;
    }

    public Task<ReturnResponseDto> CreateAsync(Guid saleId, ReturnCreateDto dto, Guid actorUserId)
    {
        return _create.ExecuteAsync(saleId, dto, actorUserId);
    }

    public Task<IReadOnlyList<ReturnResponseDto>> GetBySaleIdAsync(Guid saleId)
    {
        return _getBySaleId.ExecuteAsync(saleId);
    }

    public Task<IReadOnlyList<ReturnResponseDto>> GetByCashBoxIdAsync(Guid cashBoxId)
    {
        return _getByCashBoxId.ExecuteAsync(cashBoxId);
    }

    public Task<ReturnSummaryDto> GetSummaryByCashBoxIdAsync(Guid cashBoxId)
    {
        return _getSummaryByCashBoxId.ExecuteAsync(cashBoxId);
    }

    public Task<ReturnResponseDto> GetByIdAsync(Guid id)
    {
        return _getById.ExecuteAsync(id);
    }
}
