using WebApplication2.Models;

namespace WebApplication2.Exceptions;

public class UpdateMatchResultException : Exception
{
    public MatchEvent MatchEvent { get; }
    public string OriginalMatchResult { get; }

    public UpdateMatchResultException(MatchEvent matchEvent, string originalMatchResult) 
        : base($"Cannot apply {matchEvent} to match result: {originalMatchResult}")
    {
        MatchEvent = matchEvent;
        OriginalMatchResult = originalMatchResult;
    }

    public UpdateMatchResultException(MatchEvent matchEvent, string originalMatchResult, string message) 
        : base(message)
    {
        MatchEvent = matchEvent;
        OriginalMatchResult = originalMatchResult;
    }
} 