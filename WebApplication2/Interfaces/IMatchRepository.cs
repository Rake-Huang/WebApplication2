using WebApplication2.Models;

namespace WebApplication2.Interfaces;

public interface IMatchRepository
{
    Task<Match?> GetMatchByIdAsync(int matchId);
    Task UpdateMatchAsync(Match match);
} 