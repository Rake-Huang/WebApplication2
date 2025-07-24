using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using WebApplication2.Exceptions;
using WebApplication2.Interfaces;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Tests;

[TestFixture]
public class MatchServiceTests
{
    private MatchService _matchService;
    private IMatchRepository _mockRepository;

    [SetUp]
    public void Setup()
    {
        _mockRepository = Substitute.For<IMatchRepository>();
        _matchService = new MatchService(_mockRepository);
    }

    [Test]
    public async Task UpdateMatchResultAsync_HomeGoal_ShouldAddHToResult()
    {
        // Arrange
        var matchId = 1;
        var match = new Match { Id = matchId, MatchResult = "HHA" };
        _mockRepository.GetMatchByIdAsync(matchId).Returns(match);

        // Act
        var result = await _matchService.UpdateMatchResultAsync(matchId, MatchEvent.HomeGoal);

        // Assert
        result.Should().Be("3:1 (First Half)");
        match.MatchResult.Should().Be("HHAH");
        await _mockRepository.Received(1).UpdateMatchAsync(match);
    }

    [Test]
    public async Task UpdateMatchResultAsync_AwayGoal_ShouldAddAToResult()
    {
        // Arrange
        var matchId = 1;
        var match = new Match { Id = matchId, MatchResult = "HHA" };
        _mockRepository.GetMatchByIdAsync(matchId).Returns(match);

        // Act
        var result = await _matchService.UpdateMatchResultAsync(matchId, MatchEvent.AwayGoal);

        // Assert
        result.Should().Be("2:2 (First Half)");
        match.MatchResult.Should().Be("HHAA");
        await _mockRepository.Received(1).UpdateMatchAsync(match);
    }

    [Test]
    public async Task UpdateMatchResultAsync_NextPeriod_ShouldAddSemicolonToResult()
    {
        // Arrange
        var matchId = 1;
        var match = new Match { Id = matchId, MatchResult = "HHA" };
        _mockRepository.GetMatchByIdAsync(matchId).Returns(match);

        // Act
        var result = await _matchService.UpdateMatchResultAsync(matchId, MatchEvent.NextPeriod);

        // Assert
        result.Should().Be("2:1 (Second Half)");
        match.MatchResult.Should().Be("HHA;");
        await _mockRepository.Received(1).UpdateMatchAsync(match);
    }

    [Test]
    public async Task UpdateMatchResultAsync_SecondHalfWithGoals_ShouldReturnSecondHalf()
    {
        // Arrange
        var matchId = 1;
        var match = new Match { Id = matchId, MatchResult = "HHA;A" };
        _mockRepository.GetMatchByIdAsync(matchId).Returns(match);

        // Act
        var result = await _matchService.UpdateMatchResultAsync(matchId, MatchEvent.HomeGoal);

        // Assert
        result.Should().Be("3:2 (Second Half)");
        match.MatchResult.Should().Be("HHA;AH");
        await _mockRepository.Received(1).UpdateMatchAsync(match);
    }

    [Test]
    public void UpdateMatchResultAsync_HomeCancel_WhenLastGoalIsAway_ShouldThrowException()
    {
        // Arrange
        var matchId = 1;
        var match = new Match { Id = matchId, MatchResult = "HHA;A" };
        _mockRepository.GetMatchByIdAsync(matchId).Returns(match);

        // Act & Assert
        var exception = Assert.ThrowsAsync<UpdateMatchResultException>(() => _matchService.UpdateMatchResultAsync(matchId, MatchEvent.HomeCancel));

        exception!.MatchEvent.Should().Be(MatchEvent.HomeCancel);
        exception.OriginalMatchResult.Should().Be("HHA;A");
    }

    [Test]
    public async Task UpdateMatchResultAsync_HomeCancelWhenLastGoalIsHome_ShouldRemoveLastHomeGoal()
    {
        // Arrange
        var matchId = 1;
        var match = new Match { Id = matchId, MatchResult = "HHA;AH" };
        _mockRepository.GetMatchByIdAsync(matchId).Returns(match);

        // Act
        var result = await _matchService.UpdateMatchResultAsync(matchId, MatchEvent.HomeCancel);

        // Assert
        result.Should().Be("2:2 (Second Half)");
        match.MatchResult.Should().Be("HHA;A");
        await _mockRepository.Received(1).UpdateMatchAsync(match);
    }

    [Test]
    public async Task UpdateMatchResultAsync_HomeCancelWhenLastGoalIsHomeOnSecondHalf_ShouldRemoveLastHomeGoal()
    {
        // Arrange
        var matchId = 1;
        var match = new Match { Id = matchId, MatchResult = "HAH;" };
        _mockRepository.GetMatchByIdAsync(matchId).Returns(match);

        // Act
        var result = await _matchService.UpdateMatchResultAsync(matchId, MatchEvent.HomeCancel);

        // Assert
        result.Should().Be("1:1 (Second Half)");
        match.MatchResult.Should().Be("HA;");
        await _mockRepository.Received(1).UpdateMatchAsync(match);
    }

    [Test]
    public async Task UpdateMatchResultAsync_AwayCancel_ShouldRemoveLastAwayGoal()
    {
        // Arrange
        var matchId = 1;
        var match = new Match { Id = matchId, MatchResult = "HHA;A" };
        _mockRepository.GetMatchByIdAsync(matchId).Returns(match);

        // Act
        var result = await _matchService.UpdateMatchResultAsync(matchId, MatchEvent.AwayCancel);

        // Assert
        result.Should().Be("2:1 (Second Half)");
        match.MatchResult.Should().Be("HHA;");
        await _mockRepository.Received(1).UpdateMatchAsync(match);
    }

    [Test]
    public void UpdateMatchResultAsync_AwayCancel_WhenLastGoalIsHome_ShouldThrowException()
    {
        // Arrange
        var matchId = 1;
        var match = new Match { Id = matchId, MatchResult = "HHA;AH" };
        _mockRepository.GetMatchByIdAsync(matchId).Returns(match);

        // Act & Assert
        var exception = Assert.ThrowsAsync<UpdateMatchResultException>(() => _matchService.UpdateMatchResultAsync(matchId, MatchEvent.AwayCancel));

        exception!.MatchEvent.Should().Be(MatchEvent.AwayCancel);
        exception.OriginalMatchResult.Should().Be("HHA;AH");
    }

    [Test]
    public void UpdateMatchResultAsync_HomeCancelWithNoHomeGoals_ShouldThrowException()
    {
        // Arrange
        var matchId = 1;
        var match = new Match { Id = matchId, MatchResult = "AA;" };
        _mockRepository.GetMatchByIdAsync(matchId).Returns(match);

        // Act & Assert
        Assert.ThrowsAsync<UpdateMatchResultException>(() => _matchService.UpdateMatchResultAsync(matchId, MatchEvent.HomeCancel));
    }

    [Test]
    public void UpdateMatchResultAsync_AwayCancelWithNoAwayGoals_ShouldThrowException()
    {
        // Arrange
        var matchId = 1;
        var match = new Match { Id = matchId, MatchResult = "HH;" };
        _mockRepository.GetMatchByIdAsync(matchId).Returns(match);

        // Act & Assert
        Assert.ThrowsAsync<UpdateMatchResultException>(() => _matchService.UpdateMatchResultAsync(matchId, MatchEvent.AwayCancel));
    }

    [Test]
    public async Task UpdateMatchResultAsync_EmptyMatchResult_ShouldHandleCorrectly()
    {
        // Arrange
        var matchId = 1;
        var match = new Match { Id = matchId, MatchResult = "" };
        _mockRepository.GetMatchByIdAsync(matchId).Returns(match);

        // Act
        var result = await _matchService.UpdateMatchResultAsync(matchId, MatchEvent.HomeGoal);

        // Assert
        result.Should().Be("1:0 (First Half)");
        match.MatchResult.Should().Be("H");
    }

    [Test]
    public async Task UpdateMatchResultAsync_OnlySecondHalf_ShouldReturnSecondHalf()
    {
        // Arrange
        var matchId = 1;
        var match = new Match { Id = matchId, MatchResult = ";" };
        _mockRepository.GetMatchByIdAsync(matchId).Returns(match);

        // Act
        var result = await _matchService.UpdateMatchResultAsync(matchId, MatchEvent.HomeGoal);

        // Assert
        result.Should().Be("1:0 (Second Half)");
        match.MatchResult.Should().Be(";H");
    }
}