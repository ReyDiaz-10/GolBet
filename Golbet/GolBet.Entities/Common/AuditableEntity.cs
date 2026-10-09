// GolBet.Entities/Common/AuditableEntity.cs
namespace GolBet.Entities.Common;

/// <summary>
/// Clase base para todas las entidades del dominio.
/// Proporciona identidad, marcas de tiempo de auditoría y estado lógico de activación.
/// </summary>
public abstract class AuditableEntity
{
    public int Id { get; set; }

    /// <summary>Se establece automáticamente al guardar por primera vez.</summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>Se establece en cada actualización. Nulo hasta la primera modificación.</summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>Estado lógico. Las entidades inactivas se ocultan, no se eliminan físicamente.</summary>
    public bool IsActive { get; set; } = true;
}