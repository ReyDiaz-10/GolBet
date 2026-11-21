using GolBet.Entities;
using GolBet.Entities.Enums;

namespace GolBet.Repositories.Interfaces;

public interface IBetRepository : IGenericRepository<Bet>
{
    Task<bool> ExistsAsync(int matchId, string userId, BetPick pick);
    Task<IEnumerable<Bet>> GetByUserAsync(string userId);

    /// <summary>Pending bets of a match, TRACKED and with their users loaded</summary>
    Task<List<Bet>> GetPendingByMatchAsync(int matchId);
}