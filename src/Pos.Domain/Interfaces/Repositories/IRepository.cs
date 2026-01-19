namespace Pos.Domain.Interfaces.Repositories;

public interface IRepository <TEntity>
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<TEntity> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<IReadOnlyList<TEntity>> GetAllAsync();
}