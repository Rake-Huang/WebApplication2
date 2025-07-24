using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using WebApplication2.Controllers;
using WebApplication2.Exceptions;
using WebApplication2.Interfaces;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Tests;

[TestFixture]
public class MatchControllerTests
{
    private IMatchRepository _mockRepository;
    private MatchService _matchService;
    private MatchController _controller;

    [SetUp]
    public void Setup()
    {
        _mockRepository = Substitute.For<IMatchRepository>();
        _matchService = new MatchService(_mockRepository);
        _controller = new MatchController(_matchService);
    }

    [Test]
    public async Task UpdateMatchResult_ValidRequest_ShouldReturnResult()
    {
        // Arrange
        var matchId = 1;
        var matchEvent = MatchEvent.HomeGoal;
        var match = new Match { Id = matchId, MatchResult = "" };
        _mockRepository.GetMatchByIdAsync(matchId).Returns(match);

        // Act
        var result = await _controller.UpdateMatchResult(matchId, matchEvent);

        // Assert
        result.Should().Be("1:0 (First Half)");
    }
} 