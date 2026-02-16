using Microsoft.EntityFrameworkCore;
using Pos.Application.Interfaces.Infrastructure;

namespace Pos.Infrastructure.Data;

public sealed class TransactionalExecutor : ITransactionalExecutor
{
    private readonly PosDbContext _context;

    public TransactionalExecutor(PosDbContext context)
    {
        _context = context;
    }

    public async Task ExecuteAsync(Func<Task> operation)
    {
        await ExecuteAsync(async () =>
        {
            await operation();
            return true;
        });
    }

    public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var result = await operation();
            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
