using Pos.Application.Dtos.CashFlows;

namespace Pos.Application.Interfaces.Services;

public interface ICashFlowService
{
    Task<CashFlowResponseDto> CreateAsync(CashFlowCreateDto dto);
    Task<CashFlowResponseDto> UpdateAsync(CashFlowUpdateDto dto);
    Task<CashFlowResponseDto> GetByIdAsync(Guid id);
    Task<IReadOnlyList<CashFlowResponseDto>> GetByUserIdAsync(Guid userId);
    Task<IReadOnlyList<CashFlowResponseDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IReadOnlyList<CashFlowResponseDto>> GetAllAsync();
    Task DeleteAsync(Guid id);
}
