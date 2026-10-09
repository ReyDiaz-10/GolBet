// GolBet.Web/Controllers/MatchesController.cs
using GolBet.Entities.Enums;
using GolBet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GolBet.Web.Controllers;

public class MatchesController : Controller
{
    private readonly IMatchService _matchService;

    public MatchesController(IMatchService matchService)
        => _matchService = matchService;

    // GET /Matches
    // GET /Matches?status=Scheduled 
    public async Task<IActionResult> Index(MatchStatus? status)
    {
        ViewBag.CurrentStatus = status;
        var board = await _matchService.GetBoardAsync(status);
        return View(board);
    }

    // GET /Matches/Detail/3
    public async Task<IActionResult> Detail(int id)
    {
        var match = await _matchService.GetDetailAsync(id);

        // Si el servicio devuelve null, respondemos HTTP 404 Not Found
        if (match is null) return NotFound();

        return View(match);
    }
}