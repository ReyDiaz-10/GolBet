// GolBet.Services/Interfaces/IMatchService.cs
using GolBet.Entities.Enums;
using GolBet.Services.DTOs;

namespace GolBet.Services.Interfaces;

public interface IMatchService
{
    Task<IEnumerable<MatchDto>> GetBoardAsync(MatchStatus? status = null);
}