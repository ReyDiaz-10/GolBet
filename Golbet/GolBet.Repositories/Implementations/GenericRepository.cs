// GolBet.Repositories/Implementations/GenericRepository.cs
using GolBet.Entities.Common;
using GolBet.Repositories.Data;
using GolBet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GolBet.Repositories.Implementations;

public class GenericRepository<T> : IGenericRepository<T> where T : AuditableEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();   // Resuelve el DbSet exacto para la entidad T en tiempo de ejecución
    }

    // ---- Consultas (Lectura) ----
    public virtual async Task<IEnumerable<T>> GetAllAsync(bool includeInactive = false)
    {
        // AsNoTracking mejora el rendimiento porque EF Core no necesita vigilar cambios en consultas de solo lectura
        IQueryable<T> query = _dbSet.AsNoTracking();

        if (!includeInactive)
            query = query.Where(e => e.IsActive);

        return await query.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    // ---- Comandos (Escritura) ----
    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();   // Aquí se aplican las fechas automáticas de auditoría (Módulo 2)
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeactivateAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is null) return;

        entity.IsActive = false;             // Borrado lógico: no destruimos datos
        await _context.SaveChangesAsync();   // Al detectarse el cambio, EF Core estampa la fecha de ModifiedDate
    }
}