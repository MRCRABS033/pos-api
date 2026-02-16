using Microsoft.EntityFrameworkCore;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;
using Pos.Infrastructure.Data;

namespace Pos.Infrastructure.Repositories;

public class ReturnRepository : IReturnRepository
{
    private readonly PosDbContext _context;

    public ReturnRepository(PosDbContext context)
    {
        _context = context;
    }

    public async Task<Return> CreateAsync(Return ret)
    {
        if (ret == null)
            throw new ArgumentNullException(nameof(ret), "La devolución no puede ser nula.");

        await _context.Returns.AddAsync(ret);
        await _context.SaveChangesAsync();

        await _context.Entry(ret)
            .Collection(r => r.Items)
            .Query()
            .Include(i => i.Product)
            .LoadAsync();

        return ret;
    }

    public async Task<Return> UpdateAsync(Return ret)
    {
        if (ret is null)
            throw new ArgumentNullException(nameof(ret));

        var existing = _context.Returns.Local.FirstOrDefault(r => r.Id == ret.Id)
                       ?? await _context.Returns.FirstOrDefaultAsync(r => r.Id == ret.Id);
        if (existing is null)
            throw new KeyNotFoundException("Devolución no encontrada.");

        existing.Reason = ret.Reason;
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<Return> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id), "El id de la devolución no puede ser nulo.");

        var ret = await _context.Returns.AsNoTracking()
            .Include(r => r.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (ret is null)
            throw new KeyNotFoundException("La devolución no existe.");

        return ret;
    }

    public async Task DeleteAsync(Guid id)
    {
        var ret = await _context.Returns.FirstOrDefaultAsync(r => r.Id == id);
        if (ret is null)
            throw new KeyNotFoundException("La devolución no existe.");

        _context.Returns.Remove(ret);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Return>> GetAllAsync(int page = 1, int pageSize = 50)
    {
        var normalizedPage = Math.Max(1, page);
        var normalizedSize = Math.Clamp(pageSize, 1, 200);

        return await _context.Returns.AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedSize)
            .Take(normalizedSize)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Return>> GetBySaleId(Guid saleId)
    {
        return await _context.Returns.AsNoTracking()
            .Include(r => r.Items)
            .ThenInclude(i => i.Product)
            .Where(r => r.SaleId == saleId)
            .OrderByDescending(r => r.CreatedAt)
            .Take(500)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Return>> GetByCashBoxId(Guid cashBoxId)
    {
        return await _context.Returns.AsNoTracking()
            .Include(r => r.Items)
            .ThenInclude(i => i.Product)
            .Where(r => r.Sale.CashBoxId == cashBoxId)
            .OrderByDescending(r => r.CreatedAt)
            .Take(500)
            .ToListAsync();
    }

    public async Task<(int Count, decimal CashTotal, decimal CardTotal)> GetSummaryByCashBoxId(Guid cashBoxId)
    {
        var summary = await _context.Returns.AsNoTracking()
            .Where(r => r.Sale.CashBoxId == cashBoxId)
            .GroupBy(_ => 1)
            .Select(g => new
            {
                Count = g.Count(),
                CashTotal = g.Where(x => x.PaymentType == PaymentType.Cash).Sum(x => (decimal?)x.Total) ?? 0m,
                CardTotal = g.Where(x => x.PaymentType == PaymentType.Card).Sum(x => (decimal?)x.Total) ?? 0m
            })
            .FirstOrDefaultAsync();

        if (summary is null)
            return (0, 0m, 0m);

        return (summary.Count, summary.CashTotal, summary.CardTotal);
    }

    public async Task<Dictionary<Guid, decimal>> GetReturnedQuantitiesBySaleItemIds(IReadOnlyList<Guid> saleItemIds)
    {
        return await _context.ReturnItems.AsNoTracking()
            .Where(i => saleItemIds.Contains(i.SaleItemId))
            .GroupBy(i => i.SaleItemId)
            .Select(g => new { SaleItemId = g.Key, Qty = g.Sum(x => x.Quantity) })
            .ToDictionaryAsync(x => x.SaleItemId, x => x.Qty);
    }
}
