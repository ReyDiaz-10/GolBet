// GolBet.Services/Implementations/MatchService.cs
using AutoMapper;
using GolBet.Entities;
using GolBet.Entities.Enums;
using GolBet.Repositories.Interfaces;
using GolBet.Services.DTOs;
using GolBet.Services.Interfaces;
using GolBet.Services.Helpers;

namespace GolBet.Services.Implementations;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IBetRepository _betRepository;
    private readonly IMapper _mapper;

    private static BetPick GetWinningPick(int homeGoals, int awayGoals)
    => homeGoals > awayGoals ? BetPick.Home
     : homeGoals < awayGoals ? BetPick.Away
     : BetPick.Draw;


    public MatchService(
    IMatchRepository matchRepository,
    IBetRepository betRepository,
    IMapper mapper)
    {
        _matchRepository = matchRepository;
        _betRepository = betRepository;
        _mapper = mapper;
    }
    public async Task StartMatchAsync(int id)
    {
        var match = await _matchRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Match {id} not found.");

        if (match.Status != MatchStatus.Scheduled)
            throw new InvalidOperationException(
                "Solo un partido programado puede iniciarse.");

        match.Status = MatchStatus.InProgress;
        await _matchRepository.UpdateAsync(match);
    }

    public async Task<ResolutionSummaryDto> CloseMatchAsync(MatchResultFormDto dto)
    {
        var match = await _matchRepository.GetByIdAsync(dto.MatchId)
            ?? throw new KeyNotFoundException($"Match {dto.MatchId} not found.");

        if (match.Status == MatchStatus.Finished)
            throw new InvalidOperationException(
                "Este partido ya fue cerrado y sus apuestas resueltas.");

        if (match.Status != MatchStatus.InProgress)
            throw new InvalidOperationException(
                "El partido debe estar «En juego» para cerrarse. Inícialo primero.");

        match.HomeGoals = dto.HomeGoals;
        match.AwayGoals = dto.AwayGoals;
        match.Status = MatchStatus.Finished;

        var winningPick = GetWinningPick(dto.HomeGoals, dto.AwayGoals);

        var pendingBets = await _betRepository.GetPendingByMatchAsync(match.Id);
        var summary = new ResolutionSummaryDto { ResolvedBets = pendingBets.Count };

        foreach (var bet in pendingBets)
        {
            if (bet.Pick == winningPick)
            {
                bet.Status = BetStatus.Won;
                var payout = bet.Amount * bet.OddsAtPlacement;
                bet.User.Balance += payout;
                summary.WonBets++;
                summary.TotalPaid += payout;
            }
            else
            {
                bet.Status = BetStatus.Lost;
            }
        }

        await _matchRepository.UpdateAsync(match);

        return summary;
    }

    public async Task<IEnumerable<MatchDto>> GetBoardAsync(MatchStatus? status = null, string? search = null)
    {
        var matches = await _matchRepository.GetAllWithTeamsAsync(status, search);
        return _mapper.Map<IEnumerable<MatchDto>>(matches);
    }

    public async Task<MatchDetailDto?> GetDetailAsync(int id)
    {
        var match = await _matchRepository.GetByIdWithDetailsAsync(id);
        return match is null ? null : _mapper.Map<MatchDetailDto>(match);
    }

    public async Task<MatchFormDto?> GetForEditAsync(int id)
    {
        var match = await _matchRepository.GetByIdAsync(id);
        if (match is null) return null;
        var dto = _mapper.Map<MatchFormDto>(match);
        dto.Date = dto.Date.ToColombiaTime();        // UTC (DB) -> local (form)
        return dto;
    }

    public async Task CreateAsync(MatchFormDto dto)
    {
        ValidateBusinessRules(dto);
        var match = _mapper.Map<Match>(dto);
        match.Date = dto.Date.ToUtcFromColombia();   // local (form) -> UTC (DB)
        await _matchRepository.AddAsync(match);
    }

    public async Task UpdateAsync(MatchFormDto dto)
    {
        ValidateBusinessRules(dto);
        var match = await _matchRepository.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Match {dto.Id} not found.");

        _mapper.Map(dto, match);
        match.Date = dto.Date.ToUtcFromColombia();
        await _matchRepository.UpdateAsync(match);
    }

    public async Task DeactivateAsync(int id)
        => await _matchRepository.DeactivateAsync(id);

    // ---- Reglas de negocio (Guía del curso) ----
    private static void ValidateBusinessRules(MatchFormDto dto)
    {
        if (dto.HomeTeamId == dto.AwayTeamId)
            throw new InvalidOperationException(
                "El equipo local y el visitante no pueden ser el mismo.");
        if (dto.Date.ToUtcFromColombia() <= DateTime.UtcNow)
            throw new InvalidOperationException(
                "La fecha del partido debe ser futura.");
    }
}