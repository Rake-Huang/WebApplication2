using WebApplication2.Exceptions;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Services;

public class MatchService
{
    private readonly IMatchRepository _matchRepository;

    public MatchService(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public async Task<string> UpdateMatchResultAsync(int matchId, MatchEvent matchEvent)
    {
        var match = await _matchRepository.GetMatchByIdAsync(matchId);

        var originalResult = match!.MatchResult;

        match.MatchResult = UpdateMatchEvent(originalResult, matchEvent);
        await _matchRepository.UpdateMatchAsync(match);

        return GetDisplayResult(UpdateMatchEvent(originalResult, matchEvent));
    }

    private string UpdateMatchEvent(string currentResult, MatchEvent matchEvent)
    {
        return matchEvent switch
        {
            MatchEvent.HomeGoal => currentResult + "H",
            MatchEvent.AwayGoal => currentResult + "A",
            MatchEvent.NextPeriod => currentResult + ";",
            MatchEvent.HomeCancel => CancelLastGoal(currentResult, 'H', matchEvent),
            MatchEvent.AwayCancel => CancelLastGoal(currentResult, 'A', matchEvent),
            _ => throw new ArgumentException($"Unknown match event: {matchEvent}")
        };
    }

    private string CancelLastGoal(string currentResult, char goalType, MatchEvent matchEvent)
    {
        var lastGoalIndex = -1;
        for (int i = currentResult.Length - 1; i >= 0; i--)
        {
            if (currentResult[i] == 'H' || currentResult[i] == 'A')
            {
                lastGoalIndex = i;
                break;
            }
        }

        if (lastGoalIndex == -1)
        {
            throw new UpdateMatchResultException(matchEvent, currentResult);
        }

        if (currentResult[lastGoalIndex] != goalType)
        {
            throw new UpdateMatchResultException(matchEvent, currentResult);
        }

        return currentResult.Remove(lastGoalIndex, 1);
    }

    private string GetDisplayResult(string matchResult)
    {
        var parts = matchResult.Split(';');
        int homeGoals = 0;
        int awayGoals = 0;

        foreach (var part in parts)
        {
            homeGoals += part.Count(c => c == 'H');
            awayGoals += part.Count(c => c == 'A');
        }

        var isSecondHalf = parts.Length > 1;
        var period = isSecondHalf ? "Second Half" : "First Half";
        
        return $"{homeGoals}:{awayGoals} ({period})";
    }
} 