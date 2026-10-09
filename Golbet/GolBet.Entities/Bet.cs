// GolBet.Entities/Bet.cs
using System.ComponentModel.DataAnnotations.Schema;
using GolBet.Entities.Common;
using GolBet.Entities.Enums;

namespace GolBet.Entities;

public class Bet : AuditableEntity
{
    [Column(TypeName = "decimal(12,2)")]
    public decimal Amount { get; set; }

    /// <summary>Cuota congelada al momento de apostar.</summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal OddsAtPlacement { get; set; }

    public BetPick Pick { get; set; }
    public BetStatus Status { get; set; } = BetStatus.Pending;

    public int MatchId { get; set; }
    public Match Match { get; set; } = null!;
}