using Pos.Domain.Entities;

namespace Pos.Domain.Interfaces.Repositories;

public interface ISaleRepository : IRepository<Sale>
{
    Task<IReadOnlyList<Sale>> GetByUserId(Guid userId);
    Task<IReadOnlyList<Sale>> GetByDateRange(DateTime startDate, DateTime endDate);
}
