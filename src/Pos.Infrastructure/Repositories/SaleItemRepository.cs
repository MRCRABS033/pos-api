using Microsoft.EntityFrameworkCore;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;
using Pos.Infrastructure.Data;

namespace Pos.Infrastructure.Repositories;

public class SaleItemRepository : ISaleItemRepository
{
    private readonly PosDbContext _context;

    public SaleItemRepository(PosDbContext context)
    {
        _context = context;
    }

    public async Task<SaleItem> CreateAsync(SaleItem saleItem)
    {
        if (saleItem == null)
        {
            throw new ArgumentNullException(nameof(saleItem), "El item de venta no puede ser nulo.");
        }

        await _context.SaleItems.AddAsync(saleItem);
        await _context.SaveChangesAsync();
        return saleItem;
    }

    public async Task<SaleItem> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id), "El id del item de venta no puede ser nulo.");

        var saleItem = await _context.SaleItems.FirstOrDefaultAsync(i => i.Id == id);

        if (saleItem == null)
            throw new KeyNotFoundException("El item de venta que buscas no existe en la base de datos.");

        return saleItem;
    }

    public async Task<SaleItem> UpdateAsync(SaleItem saleItem)
    {
        if (saleItem is null)
            throw new ArgumentNullException(nameof(saleItem));

        var existing = await _context.SaleItems.FirstOrDefaultAsync(i => i.Id == saleItem.Id);
        if (existing is null)
            throw new KeyNotFoundException("Item de venta no encontrado.");

        existing.SaleId = saleItem.SaleId;
        existing.ProductId = saleItem.ProductId;
        existing.Quantity = saleItem.Quantity;
        existing.UnitPrice = saleItem.UnitPrice;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteAsync(Guid id)
    {
        var saleItem = await _context.SaleItems.FirstOrDefaultAsync(i => i.Id == id);
        if (saleItem is null)
            throw new KeyNotFoundException("Item de venta no encontrado.");
        _context.SaleItems.Remove(saleItem);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<SaleItem>> GetBySaleId(Guid saleId)
    {
        return await _context.SaleItems.AsNoTracking()
            .Where(i => i.SaleId == saleId)
            .OrderBy(i => i.Id)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<SaleItem>> GetByProductId(Guid productId)
    {
        return await _context.SaleItems.AsNoTracking()
            .Where(i => i.ProductId == productId)
            .OrderBy(i => i.Id)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<SaleItem>> GetAllAsync()
    {
        return await _context.SaleItems.AsNoTracking()
            .OrderBy(i => i.Id)
            .Take(100)
            .ToListAsync();
    }
}
