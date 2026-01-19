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
        return product;
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id), "El id del producot no puede ser nulo.");
        
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        
        if (product == null)
            throw new KeyNotFoundException("El producto que buscas no existe en la base de datos.");
        
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        if (product is null)
            throw new ArgumentNullException(nameof(product));

        var existing = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
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
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
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

    public async Task<Product> GetByProductSku(string sku)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Sku == sku);
        if (product is null)
            throw new KeyNotFoundException("Producto no encontrado.");
        return product;
    }
}