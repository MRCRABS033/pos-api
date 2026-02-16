using Pos.Application.Dtos.Sales;

namespace Pos.Application.Interfaces.Services;

public interface ISaleService
{
    Task<SaleResponseDto> CreateAsync(SaleCreateDto dto, Guid userId);
    Task<SaleResponseDto> UpdateAsync(SaleUpdateDto dto, Guid userId);
    Task<SaleResponseDto> GetByIdAsync(Guid id);
    Task<IReadOnlyList<SaleResponseDto>> GetByUserIdAsync(Guid userId);
    Task<IReadOnlyList<SaleResponseDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IReadOnlyList<SaleResponseDto>> GetByCashBoxIdAsync(Guid cashBoxId);
    Task<IReadOnlyList<SaleResponseDto>> GetAllAsync(int page = 1, int pageSize = 50);
    Task DeleteAsync(Guid id);
}
