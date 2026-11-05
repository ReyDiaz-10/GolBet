using GolBet.Entities.Enums;

namespace GolBet.Services.DTOs;

public class BetDto
{
    public int Id { get; set; }
    public string MatchHomeTeamName { get; set; } = null!;
    public string MatchAwayTeamName { get; set; } = null!;
    public DateTime MatchDate { get; set; }
    public BetPick Pick { get; set; }
    public decimal Amount { get; set; }
    public decimal OddsAtPlacement { get; set; }
    public decimal PotentialPayout { get; set; }
    public BetStatus Status { get; set; }
}