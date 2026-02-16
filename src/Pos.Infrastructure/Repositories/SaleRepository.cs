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

        var existing = _context.Sales.Local.FirstOrDefault(s => s.Id == sale.Id)
                       ?? await _context.Sales.FirstOrDefaultAsync(s => s.Id == sale.Id);
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
            .Take(500)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Sale>> GetByDateRange(DateTime startDate, DateTime endDate)
    {
        return await _context.Sales.AsNoTracking()
            .Where(s => s.CreatedAt >= startDate && s.CreatedAt <= endDate)
            .OrderByDescending(s => s.CreatedAt)
            .Take(500)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Sale>> GetByCashBoxId(Guid cashBoxId)
    {
        return await _context.Sales.AsNoTracking()
            .Where(s => s.CashBoxId == cashBoxId)
            .OrderByDescending(s => s.CreatedAt)
            .Take(500)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<SaleWithItemsCount>> GetByCashBoxIdWithItemsCount(Guid cashBoxId)
    {
        return await _context.Sales.AsNoTracking()
            .Where(s => s.CashBoxId == cashBoxId)
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => new SaleWithItemsCount(s, s.Items.Count))
            .Take(500)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<SalesByDepartmentRow>> GetSalesByDepartmentByCashBoxId(Guid cashBoxId)
    {
        var grouped = await _context.SaleItems.AsNoTracking()
            .Where(i => i.Sale.CashBoxId == cashBoxId)
            .GroupBy(i => new
            {
                i.Product.CategoryId,
                CategoryName = i.Product.Category == null ? null : i.Product.Category.Name
            })
            .Select(group => new
            {
                group.Key.CategoryId,
                group.Key.CategoryName,
                TotalSales = group.Sum(x => x.Quantity * x.UnitPrice),
                ItemsCount = group.Count()
            })
            .OrderByDescending(x => x.TotalSales)
            .ToListAsync();

        return grouped
            .Select(x => new SalesByDepartmentRow(
                x.CategoryId,
                x.CategoryName,
                x.TotalSales,
                x.ItemsCount))
            .ToList();
    }

    public async Task<IReadOnlyList<Sale>> GetAllAsync(int page = 1, int pageSize = 50)
    {
        var normalizedPage = Math.Max(1, page);
        var normalizedSize = Math.Clamp(pageSize, 1, 200);

        return await _context.Sales.AsNoTracking()
            .OrderByDescending(s => s.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedSize)
            .Take(normalizedSize)
            .ToListAsync();
    }
}
