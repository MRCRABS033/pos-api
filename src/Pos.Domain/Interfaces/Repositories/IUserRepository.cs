using Pos.Domain.Entities;

namespace Pos.Domain.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmail(string email);
    Task<User?> GetByNormaliceName(string normaliceName);
    Task<bool> ExistsOwnerAsync();
    Task EnsurePermissionCatalogAsync();
    Task<IReadOnlyList<string>> GetAllPermissionCodesAsync();
    Task<IReadOnlyList<string>> GetPermissionCodes(Guid userId);
    Task SetDefaultPermissionsByRole(Guid userId, string role);
    Task SetPermissionCodes(Guid userId, IReadOnlyList<string> codes);
}
