using Microsoft.EntityFrameworkCore;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;
using Pos.Infrastructure.Data;

namespace Pos.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly PosDbContext _context;

    public ProductRepository(PosDbContext context)
    {
        _context = context;
    }
    public async Task<Product> CreateAsync(Product product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product) , "El producto no puede ser nulo.");
        }

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        await _context.Entry(product).Reference(p => p.Category).LoadAsync();
        return product;
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id), "El id del producot no puede ser nulo.");
        
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (product == null)
            throw new KeyNotFoundException("El producto que buscas no existe en la base de datos.");
        
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        if (product is null)
            throw new ArgumentNullException(nameof(product));

        var existing = _context.Products.Local.FirstOrDefault(p => p.Id == product.Id)
                       ?? await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
        if (existing is null)
            throw new KeyNotFoundException("Producto no encontrado.");

        existing.Name = product.Name;
        existing.Sku = product.Sku;
        existing.PriceCost = product.PriceCost;
        existing.PriceSell = product.PriceSell;
        existing.Stock = product.Stock;
        existing.IsActive = product.IsActive;
        existing.IsAvailable = product.IsAvailable;
        existing.CategoryId = product.CategoryId;
        existing.UpdatedById = product.UpdatedById;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        await _context.Entry(existing).Reference(p => p.Category).LoadAsync();
        return existing;
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product is null)
            throw new KeyNotFoundException("Producto no encontrado.");
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        
    }

    public async Task<Product?> GetByProductSku(string sku)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Sku == sku);
        if (product is null)
            throw new KeyNotFoundException("Producto no encontrado.");
        return product;
    }

    public async Task<IReadOnlyList<Product>> GetByProductName(string term)
    {
        term = term?.Trim() ?? string.Empty;
        if (term.Length == 0) return Array.Empty<Product>();

        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => EF.Functions.ILike(p.Name, $"%{term}%"))
            .OrderBy(p => p.Name)
            .Take(100)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Product>> GetByCategoryId(Guid categoryId)
    {
        var products = await _context.Products.AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .OrderBy(p => p.Name)
            .Take(500)
            .ToListAsync();
        return products;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(int page = 1, int pageSize = 50)
    {
        var normalizedPage = Math.Max(1, page);
        var normalizedSize = Math.Clamp(pageSize, 1, 200);

        return await _context.Products.AsNoTracking()
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .Skip((normalizedPage - 1) * normalizedSize)
            .Take(normalizedSize)
            .ToListAsync();
    }

}
