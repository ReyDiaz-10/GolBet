// GolBet.Services/DTOs/MatchDetailDto.cs
namespace GolBet.Services.DTOs;

/// <summary>
/// Modelo de lectura para la página de detalle.
/// Hereda todo lo de la cartelera y agrega datos exclusivos del detalle.
/// </summary>
public class MatchDetailDto : MatchDto
{
    /// <summary>Cuántas apuestas se han realizado en este partido.</summary>
    public int TotalBets { get; set; }
}