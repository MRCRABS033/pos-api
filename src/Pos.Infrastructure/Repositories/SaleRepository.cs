using Microsoft.EntityFrameworkCore;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;
using Pos.Infrastructure.Data;

namespace Pos.Infrastructure.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly PosDbContext _context;

    public SaleRepository(PosDbContext context)
    {
        _context = context;
    }

    public async Task<Sale> CreateAsync(Sale sale)
    {
        if (sale == null)
        {
            throw new ArgumentNullException(nameof(sale), "La venta no puede ser nula.");
        }

        await _context.Sales.AddAsync(sale);
        await _context.SaveChangesAsync();
        return sale;
    }

    public async Task<Sale> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id), "El id de la venta no puede ser nulo.");

        var sale = await _context.Sales.FirstOrDefaultAsync(s => s.Id == id);

        if (sale == null)
            throw new KeyNotFoundException("La venta que buscas no existe en la base de datos.");

        return sale;
    }

    public async Task<Sale> UpdateAsync(Sale sale)
    {
        if (sale is null)
            throw new ArgumentNullException(nameof(sale));

        var existing = await _context.Sales.FirstOrDefaultAsync(s => s.Id == sale.Id);
        if (existing is null)
            throw new KeyNotFoundException("Venta no encontrada.");

        existing.UserId = sale.UserId;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteAsync(Guid id)
    {
        var sale = await _context.Sales.FirstOrDefaultAsync(s => s.Id == id);
        if (sale is null)
            throw new KeyNotFoundException("Venta no encontrada.");
        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Sale>> GetByUserId(Guid userId)
    {
        return await _context.Sales.AsNoTracking()
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Sale>> GetByDateRange(DateTime startDate, DateTime endDate)
    {
        return await _context.Sales.AsNoTracking()
            .Where(s => s.CreatedAt >= startDate && s.CreatedAt <= endDate)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Sale>> GetAllAsync()
    {
        return await _context.Sales.AsNoTracking()
            .OrderByDescending(s => s.CreatedAt)
            .Take(100)
            .ToListAsync();
    }
}
