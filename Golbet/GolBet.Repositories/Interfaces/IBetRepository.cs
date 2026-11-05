using GolBet.Entities;
using GolBet.Entities.Enums;

namespace GolBet.Repositories.Interfaces;

public interface IBetRepository : IGenericRepository<Bet>
{
    Task<bool> ExistsAsync(int matchId, string userId, BetPick pick);
    Task<IEnumerable<Bet>> GetByUserAsync(string userId);
}