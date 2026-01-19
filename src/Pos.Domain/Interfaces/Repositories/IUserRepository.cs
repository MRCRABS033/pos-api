using Pos.Domain.Entities;

namespace Pos.Domain.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmail(string email);
    Task<User?> GetByNormaliceName(string normaliceName);
}
