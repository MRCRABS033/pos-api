using Pos.Domain.Entities;

namespace Pos.Domain.Interfaces.Repositories;

public interface IReturnRepository : IRepository<Return>
{
    Task<IReadOnlyList<Return>> GetBySaleId(Guid saleId);
    Task<IReadOnlyList<Return>> GetByCashBoxId(Guid cashBoxId);
    Task<(int Count, decimal CashTotal, decimal CardTotal)> GetSummaryByCashBoxId(Guid cashBoxId);
    Task<Dictionary<Guid, decimal>> GetReturnedQuantitiesBySaleItemIds(IReadOnlyList<Guid> saleItemIds);
}
