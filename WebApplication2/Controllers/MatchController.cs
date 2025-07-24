using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchController : ControllerBase
{
    private readonly MatchService _matchService;

    public MatchController(MatchService matchService)
    {
        _matchService = matchService;
    }

    [HttpPost("updateMatchResult")]
    public async Task<string> UpdateMatchResult(int matchId, MatchEvent matchEvent)
    {
        return await _matchService.UpdateMatchResultAsync(matchId, matchEvent);
    }
} 