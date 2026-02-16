using Pos.Domain.Entities;

namespace Pos.Domain.Interfaces.Repositories;

public interface ISaleRepository : IRepository<Sale>
{
    Task<IReadOnlyList<Sale>> GetByUserId(Guid userId);
    Task<IReadOnlyList<Sale>> GetByDateRange(DateTime startDate, DateTime endDate);
    Task<IReadOnlyList<Sale>> GetByCashBoxId(Guid cashBoxId);
    Task<IReadOnlyList<SaleWithItemsCount>> GetByCashBoxIdWithItemsCount(Guid cashBoxId);
    Task<IReadOnlyList<SalesByDepartmentRow>> GetSalesByDepartmentByCashBoxId(Guid cashBoxId);
}
