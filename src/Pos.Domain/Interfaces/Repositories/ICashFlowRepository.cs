using Pos.Domain.Entities;

namespace Pos.Domain.Interfaces.Repositories;

public interface ICashFlowRepository : IRepository<CashFlow>
{
    Task<IReadOnlyList<CashFlow>> GetByUserId(Guid userId);
    Task<IReadOnlyList<CashFlow>> GetByDateRange(DateTime startDate, DateTime endDate);
}
