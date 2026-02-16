using Pos.Domain.Entities;

namespace Pos.Domain.Interfaces.Repositories;

public interface ICashFlowRepository : IRepository<CashFlow>
{
    Task<IReadOnlyList<CashFlow>> GetByUserId(Guid userId);
    Task<IReadOnlyList<CashFlow>> GetByDateRange(DateTime startDate, DateTime endDate);
    Task<IReadOnlyList<CashFlow>> GetByCashBoxId(Guid cashBoxId);
    Task<(int EntriesCount, int ExitsCount, decimal EntriesTotal, decimal ExitsTotal)> GetSummaryByCashBoxId(Guid cashBoxId);
}
