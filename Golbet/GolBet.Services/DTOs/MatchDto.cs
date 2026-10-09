// GolBet.Services/DTOs/MatchDto.cs
using GolBet.Entities.Enums;

namespace GolBet.Services.DTOs;

/// <summary>
/// Modelo de lectura plano para la cartelera. No contiene propiedades de navegación.
/// </summary>
public class MatchDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public MatchStatus Status { get; set; }

    // Nombres construidos intencionalmente para que AutoMapper los aplane
    public string HomeTeamName { get; set; } = null!;
    public string? HomeTeamCrestUrl { get; set; }
    public string AwayTeamName { get; set; } = null!;
    public string? AwayTeamCrestUrl { get; set; }

    public int? HomeGoals { get; set; }
    public int? AwayGoals { get; set; }

    public decimal HomeOdds { get; set; }
    public decimal DrawOdds { get; set; }
    public decimal AwayOdds { get; set; }
}