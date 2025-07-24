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

    [Test]
    public void query_budget_when_get_cross_month()
    {
        GivenBudget(
            CreateBudget("202507", 620)
            , CreateBudget("202506", 300)
        );
        var amount = _budgetService.Query(new DateTime(2025, 06, 01), new DateTime(2025, 07, 31));
        amount.Should().Be(920);
    }

    [Test]
    public void query_budget_when_get_partial_cross_month()
    {
        GivenBudget(
            CreateBudget("202506", 300),
            CreateBudget("202507", 310)
        );
        var amount = _budgetService.Query(new DateTime(2025, 06, 21), new DateTime(2025, 07, 10));
        amount.Should().Be(200);
    }

    [Test]
    public void query_budget_when_get_cross_year()
    {
        GivenBudget(
            CreateBudget("202412", 310),
            CreateBudget("202501", 310)
        );
        var amount = _budgetService.Query(new DateTime(2024, 12, 01), new DateTime(2025, 01, 31));
        amount.Should().Be(620);
    }

    [Test]
    public void query_budget_when_get_partial_cross_year()
    {
        GivenBudget(
            CreateBudget("202412", 310),
            CreateBudget("202501", 310)
        );
        var amount = _budgetService.Query(new DateTime(2024, 12, 31), new DateTime(2025, 01, 01));
        amount.Should().Be(10 + 10);
    }

    [Test]
    public void query_budget_with_no_budget_data()
    {
        GivenBudget();
        var amount = _budgetService.Query(new DateTime(2025, 01, 01), new DateTime(2025, 01, 31));
        amount.Should().Be(0);
    }

    [Test]
    public void query_budget_with_invalid_date_range()
    {
        GivenBudget(CreateBudget("202501", 310));
        var amount = _budgetService.Query(new DateTime(2025, 01, 31), new DateTime(2025, 01, 01));
        amount.Should().Be(0);
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