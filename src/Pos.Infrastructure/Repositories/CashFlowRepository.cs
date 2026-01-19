using Microsoft.EntityFrameworkCore;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;
using Pos.Infrastructure.Data;

namespace Pos.Infrastructure.Repositories;

public class CashFlowRepository : ICashFlowRepository
{
    private readonly PosDbContext _context;

    public CashFlowRepository(PosDbContext context)
    {
        _context = context;
    }

    public async Task<CashFlow> CreateAsync(CashFlow cashFlow)
    {
        if (cashFlow == null)
        {
            throw new ArgumentNullException(nameof(cashFlow), "El flujo de caja no puede ser nulo.");
        }

        await _context.CashFlows.AddAsync(cashFlow);
        await _context.SaveChangesAsync();
        return cashFlow;
    }

    public async Task<CashFlow> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id), "El id del flujo de caja no puede ser nulo.");

        var cashFlow = await _context.CashFlows.FirstOrDefaultAsync(c => c.Id == id);

        if (cashFlow == null)
            throw new KeyNotFoundException("El flujo de caja que buscas no existe en la base de datos.");

        return cashFlow;
    }

    public async Task<CashFlow> UpdateAsync(CashFlow cashFlow)
    {
        if (cashFlow is null)
            throw new ArgumentNullException(nameof(cashFlow));

        var existing = await _context.CashFlows.FirstOrDefaultAsync(c => c.Id == cashFlow.Id);
        if (existing is null)
            throw new KeyNotFoundException("Flujo de caja no encontrado.");

        existing.Motive = cashFlow.Motive;
        existing.Amount = cashFlow.Amount;
        existing.UserId = cashFlow.UserId;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteAsync(Guid id)
    {
        var cashFlow = await _context.CashFlows.FirstOrDefaultAsync(c => c.Id == id);
        if (cashFlow is null)
            throw new KeyNotFoundException("Flujo de caja no encontrado.");
        _context.CashFlows.Remove(cashFlow);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<CashFlow>> GetByUserId(Guid userId)
    {
        return await _context.CashFlows.AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<CashFlow>> GetByDateRange(DateTime startDate, DateTime endDate)
    {
        return await _context.CashFlows.AsNoTracking()
            .Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<CashFlow>> GetAllAsync()
    {
        return await _context.CashFlows.AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .Take(100)
            .ToListAsync();
    }
}
