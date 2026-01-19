using Pos.Domain.Entities;

namespace Pos.Domain.Interfaces.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByName(string name);
}
