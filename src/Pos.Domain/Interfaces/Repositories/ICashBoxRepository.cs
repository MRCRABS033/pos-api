using Pos.Domain.Entities;

namespace Pos.Domain.Interfaces.Repositories;

public interface ICashBoxRepositry : IRepository<CashBox>
{
    public Task<CashBox?> GetByDateAsync(DateTime date);
    public Task<IReadOnlyList<CashBox>> GetAllByUserId(Guid userId);
}