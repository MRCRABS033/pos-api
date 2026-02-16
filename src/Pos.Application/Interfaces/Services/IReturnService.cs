using Pos.Application.Dtos.Returns;

namespace Pos.Application.Interfaces.Services;

public interface IReturnService
{
    Task<ReturnResponseDto> CreateAsync(Guid saleId, ReturnCreateDto dto, Guid actorUserId);
    Task<IReadOnlyList<ReturnResponseDto>> GetBySaleIdAsync(Guid saleId);
    Task<IReadOnlyList<ReturnResponseDto>> GetByCashBoxIdAsync(Guid cashBoxId);
    Task<ReturnSummaryDto> GetSummaryByCashBoxIdAsync(Guid cashBoxId);
    Task<ReturnResponseDto> GetByIdAsync(Guid id);
}
