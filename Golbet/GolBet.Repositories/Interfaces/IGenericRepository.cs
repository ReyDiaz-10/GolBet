// GolBet.Repositories/Interfaces/IGenericRepository.cs
using GolBet.Entities.Common;

namespace GolBet.Repositories.Interfaces;

/// <summary>
/// Contrato genérico de acceso a datos para todas las entidades del dominio.
/// Las consultas específicas vivirán en repositorios propios de cada entidad.
/// </summary>
public interface IGenericRepository<T> where T : AuditableEntity
{
    // ---- Consultas (Lectura) ----
    Task<IEnumerable<T>> GetAllAsync(bool includeInactive = false);
    Task<T?> GetByIdAsync(int id);

    // ---- Comandos (Escritura) ----
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeactivateAsync(int id);   // Borrado lógico: IsActive = false
}