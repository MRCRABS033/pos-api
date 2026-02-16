using Pos.Application.Dtos.SaleItems;

namespace Pos.Application.Interfaces.Services;

public interface ISaleItemService
{
    Task<SaleItemResponseDto> CreateAsync(SaleItemCreateDto dto, Guid saleId);
    Task<SaleItemResponseDto> UpdateAsync(SaleItemUpdateDto dto);
    Task<SaleItemResponseDto> GetByIdAsync(Guid id);
    Task<IReadOnlyList<SaleItemResponseDto>> GetBySaleIdAsync(Guid saleId);
    Task<IReadOnlyList<SaleItemResponseDto>> GetByProductIdAsync(Guid productId);
    Task<IReadOnlyList<SaleItemResponseDto>> GetAllAsync(int page = 1, int pageSize = 50);
    Task DeleteAsync(Guid id);
}
