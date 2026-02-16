using Pos.Domain.Entities;

namespace Pos.Domain.Interfaces.Repositories;

public interface IUserActivityLogRepository
{
    Task CreateAsync(UserActivityLog activity);
    Task<IReadOnlyList<UserActivityLog>> GetByCashBoxAndUserAsync(Guid cashBoxId, Guid userId);
    Task<IReadOnlyList<UserActivityLog>> GetByCashBoxAsync(Guid cashBoxId);
}
