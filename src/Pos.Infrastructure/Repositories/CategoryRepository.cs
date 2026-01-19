using Microsoft.EntityFrameworkCore;
using Pos.Domain.Entities;
using Pos.Domain.Interfaces.Repositories;
using Pos.Infrastructure.Data;

namespace Pos.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly PosDbContext _context;

    public CategoryRepository(PosDbContext context)
    {
        _context = context;
    }

    public async Task<Category> CreateAsync(Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category), "La categoria no puede ser nula.");
        }

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id), "El id de la categoria no puede ser nulo.");

        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            throw new KeyNotFoundException("La categoria que buscas no existe en la base de datos.");

        return category;
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        if (category is null)
            throw new ArgumentNullException(nameof(category));

        var existing = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
        if (existing is null)
            throw new KeyNotFoundException("Categoria no encontrada.");

        existing.Name = category.Name;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category is null)
            throw new KeyNotFoundException("Categoria no encontrada.");
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<Category?> GetByName(string name)
    {
        var category = await _context.Categories.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name);

        if (category is null)
            throw new KeyNotFoundException("Categoria no encontrada.");

        return category;
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync()
    {
        return await _context.Categories.AsNoTracking()
            .OrderBy(c => c.Name)
            .Take(100)
            .ToListAsync();
    }
}
