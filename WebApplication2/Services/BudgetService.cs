using WebApplication2.Repositories;

namespace WebApplication2.Services;

public class BudgetService(IBudgetRepo budgetRepo)
{
    public decimal Query(DateTime startTime, DateTime endTime)
    {
        var budgets = budgetRepo.GetAll();
        var startYearMonth = $"{startTime:yyyyMM}";
        var budget = budgets.FirstOrDefault(e => e.YearMonth == startYearMonth);
        
        var daysInMonth = DateTime.DaysInMonth(startTime.Year, startTime.Month);
        var effectiveDays = (endTime - startTime).Days + 1;
        
        return Math.Round((decimal)(budget.Amount * effectiveDays / daysInMonth), 0);
    }
}