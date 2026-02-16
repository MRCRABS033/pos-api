using Microsoft.EntityFrameworkCore;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;
using Pos.Infrastructure.Data;

namespace Pos.Infrastructure.Repositories;

public class UserActivityLogRepository : IUserActivityLogRepository
{
    private readonly PosDbContext _context;

    public UserActivityLogRepository(PosDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(UserActivityLog activity)
    {
        if (activity is null)
            throw new ArgumentNullException(nameof(activity));

        await _context.UserActivityLogs.AddAsync(activity);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<UserActivityLog>> GetByCashBoxAndUserAsync(Guid cashBoxId, Guid userId)
    {
        return await _context.UserActivityLogs.AsNoTracking()
            .Where(a => a.CashBoxId == cashBoxId && a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<UserActivityLog>> GetByCashBoxAsync(Guid cashBoxId)
    {
        return await _context.UserActivityLogs.AsNoTracking()
            .Where(a => a.CashBoxId == cashBoxId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }
}
