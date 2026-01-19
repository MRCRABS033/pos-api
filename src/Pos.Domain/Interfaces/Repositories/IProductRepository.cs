using Pos.Domain.Entities;

namespace Pos.Domain.Interfaces.Repositories;

public interface IProductRepository: IRepository<Product>
{
    Task<IReadOnlyList<Product>> GetByCategoryId(Guid categoryId);
    Task<Product?> GetByProductSku(string sku);
    Task<IReadOnlyList<Product>> GetByProductName(string name);
}
