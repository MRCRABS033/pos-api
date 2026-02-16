using Pos.Application.Dtos.CashFlows;

namespace Pos.Application.Interfaces.Services;

public interface ICashFlowService
{
    Task<CashFlowResponseDto> CreateAsync(CashFlowCreateDto dto, Guid actorUserId);
    Task<CashFlowResponseDto> UpdateAsync(CashFlowUpdateDto dto, Guid actorUserId);
    Task<CashFlowResponseDto> GetByIdAsync(Guid id);
    Task<IReadOnlyList<CashFlowResponseDto>> GetByUserIdAsync(Guid userId);
    Task<IReadOnlyList<CashFlowResponseDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IReadOnlyList<CashFlowResponseDto>> GetByCashBoxIdAsync(Guid cashBoxId);
    Task<CashFlowSummaryDto> GetSummaryByCashBoxIdAsync(Guid cashBoxId);
    Task<IReadOnlyList<CashFlowResponseDto>> GetAllAsync(int page = 1, int pageSize = 50);
    Task DeleteAsync(Guid id);
}
