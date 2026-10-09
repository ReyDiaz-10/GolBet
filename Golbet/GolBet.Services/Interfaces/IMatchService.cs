// GolBet.Services/Interfaces/IMatchService.cs
using GolBet.Entities.Enums;
using GolBet.Services.DTOs;

namespace GolBet.Services.Interfaces;

public interface IMatchService
{
    /// <summary>
    /// Obtiene la cartelera de partidos activos, opcionalmente filtrada por estado.
    /// </summary>
    Task<IEnumerable<MatchDto>> GetBoardAsync(MatchStatus? status = null);

    /// <summary>
    /// Obtiene el detalle completo de un partido por su identificador.
    /// </summary>
    Task<MatchDetailDto?> GetDetailAsync(int id);

    /// <summary>
    /// Obtiene el formulario DTO de un partido para su edición.
    /// </summary>
    Task<MatchFormDto?> GetForEditAsync(int id);

    /// <summary>
    /// Crea un nuevo partido validando las reglas de negocio.
    /// </summary>
    Task CreateAsync(MatchFormDto dto);

    /// <summary>
    /// Actualiza un partido existente.
    /// </summary>
    Task UpdateAsync(MatchFormDto dto);

    /// <summary>
    /// Realiza el borrado lógico de un partido cambiando su estado activo.
    /// </summary>
    Task DeactivateAsync(int id);
}