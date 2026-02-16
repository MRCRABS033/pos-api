namespace Pos.Application.Interfaces.Infrastructure;

public interface ITransactionalExecutor
{
    Task ExecuteAsync(Func<Task> operation);
    Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation);
}
