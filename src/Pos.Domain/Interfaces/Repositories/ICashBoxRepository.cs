using Pos.Domain.Entities;

namespace Pos.Domain.Interfaces.Repositories;

public interface ICashBoxRepository : IRepository<CashBox>
{
    public Task<IReadOnlyList<CashBox>> GetByDateRangeAsync(DateTime start, DateTime end);
    public Task<IReadOnlyList<CashBox>> GetAllByUserId(Guid userId);
    public Task<CashBox?> GetLatestByUserId(Guid userId);
}
