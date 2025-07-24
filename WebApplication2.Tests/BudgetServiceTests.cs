using FluentAssertions;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using WebApplication2.Models;
using WebApplication2.Repositories;
using WebApplication2.Services;

namespace WebApplication2.Tests;

[TestFixture]
public class BudgetServiceTests
{
    private IBudgetRepo _budgetRepo;
    private BudgetService _budgetService;

    [SetUp]
    public void SetUp()
    {
        _budgetRepo = Substitute.For<IBudgetRepo>();
        _budgetService = new BudgetService(_budgetRepo);
    }

    [Test]
    public void query_budget_success()
    {
        GivenBudget(CreateBudget("202507", 310));
        var amount = _budgetService.Query(new DateTime(2025, 07, 01), new DateTime(2025, 07, 31));
        amount.Should().Be(310);
    }

    [Test]
    public void query_budget_when_get_partial_month()
    {
        GivenBudget(CreateBudget("202507", 310));
        var amount = _budgetService.Query(new DateTime(2025, 07, 16), new DateTime(2025, 07, 31));
        amount.Should().Be(160);
    }


    private static Budget CreateBudget(string yearMonth, int amount)
    {
        return new Budget
        {
            YearMonth = yearMonth,
            Amount = amount
        };
    }

    private ConfiguredCall GivenBudget(params Budget[] budgets)
    {
        return _budgetRepo.GetAll().Returns(budgets.ToList());
    }
}