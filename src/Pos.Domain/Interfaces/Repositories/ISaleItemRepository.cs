using Pos.Domain.Entities;

namespace Pos.Domain.Interfaces.Repositories;

public interface ISaleItemRepository : IRepository<SaleItem>
{
    Task<IReadOnlyList<SaleItem>> GetBySaleId(Guid saleId);
    Task<IReadOnlyList<SaleItem>> GetByProductId(Guid productId);
}
