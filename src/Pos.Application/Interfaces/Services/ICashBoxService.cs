using Pos.Application.Dtos.CashBoxes;

namespace Pos.Application.Interfaces.Services;

public interface ICashBoxService
{
    Task<CashBoxResponseDto> CreateAsync(CashBoxCreateDto dto, Guid actorUserId);
    Task<CashBoxResponseDto> UpdateAsync(CashBoxUpdateDto dto, Guid actorUserId);
    Task<CashBoxResponseDto> GetByIdAsync(Guid id);
    Task<IReadOnlyList<CashBoxResponseDto>> GetByDateAsync(DateTime date);
    Task<CashBoxResponseDto> GetLatestByUserIdAsync(Guid userId);
    Task<IReadOnlyList<CashBoxResponseDto>> GetByUserIdAsync(Guid userId);
    Task<IReadOnlyList<CashBoxResponseDto>> GetAllAsync(int page = 1, int pageSize = 50);
    Task<EmployeeShiftSummaryDto> GetEmployeeShiftSummaryAsync(Guid cashBoxId, Guid userId);
    Task<ShiftCutSummaryDto> GetShiftCutSummaryAsync(Guid cashBoxId);
    Task DeleteAsync(Guid id);
}
