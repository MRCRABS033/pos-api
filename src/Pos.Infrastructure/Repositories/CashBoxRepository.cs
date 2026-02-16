using Microsoft.EntityFrameworkCore;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;
using Pos.Infrastructure.Data;

namespace Pos.Infrastructure.Repositories;

public class CashBoxRepository : ICashBoxRepository
{
    private readonly PosDbContext _context;
    public CashBoxRepository(PosDbContext context)
    {
        _context = context;
    }

    public async Task<CashBox> CreateAsync(CashBox cashBox)
    {
        if (cashBox == null)
            throw new ArgumentNullException(nameof(cashBox), "La Caja no se pudo registrar.");

        await _context.CashBoxes.AddAsync(cashBox);
        await _context.SaveChangesAsync();
        return cashBox;
    }

    public async Task<CashBox> UpdateAsync(CashBox cashBox)
    {
        if (cashBox is null)
            throw new ArgumentNullException(nameof(cashBox));

        var existing = _context.CashBoxes.Local.FirstOrDefault(c => c.Id == cashBox.Id)
                       ?? await _context.CashBoxes.FirstOrDefaultAsync(c => c.Id == cashBox.Id);
        if (existing is null)
            throw new KeyNotFoundException("Caja no encontrada.");

        existing.UserId = cashBox.UserId;
        existing.Status = cashBox.Status;
        existing.OpeningBalance = cashBox.OpeningBalance;
        existing.CashTotal = cashBox.CashTotal;
        existing.CardTotal = cashBox.CardTotal;
        existing.ExpectatedBalance = cashBox.ExpectatedBalance;
        existing.ActualBalance = cashBox.ActualBalance;
        existing.TicketsCount = cashBox.TicketsCount;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<CashBox> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id), "El id de la caja no puede ser nulo.");

        var cashBox = await _context.CashBoxes.FirstOrDefaultAsync(c => c.Id == id);
        if (cashBox is null)
            throw new KeyNotFoundException("La caja que buscas no existe en la base de datos.");

        return cashBox;
    }

    public async Task DeleteAsync(Guid id)
    {
        var cashBox = await _context.CashBoxes.FirstOrDefaultAsync(c => c.Id == id);
        if (cashBox is null)
            throw new KeyNotFoundException("Caja no encontrada.");
        _context.CashBoxes.Remove(cashBox);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<CashBox>> GetAllAsync(int page = 1, int pageSize = 50)
    {
        var normalizedPage = Math.Max(1, page);
        var normalizedSize = Math.Clamp(pageSize, 1, 200);

        return await _context.CashBoxes.AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedSize)
            .Take(normalizedSize)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<CashBox>> GetByDateRangeAsync(DateTime start, DateTime end)
    {
        return await _context.CashBoxes.AsNoTracking()
            .Where(c => c.CreatedAt >= start && c.CreatedAt < end)
            .OrderByDescending(c => c.CreatedAt)
            .Take(500)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<CashBox>> GetAllByUserId(Guid userId)
    {
        return await _context.CashBoxes.AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .Take(500)
            .ToListAsync();
    }

    public async Task<CashBox?> GetLatestByUserId(Guid userId)
    {
        return await _context.CashBoxes.AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync();
    }
}
