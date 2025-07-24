using WebApplication2.Repositories;

namespace WebApplication2.Services;


public class BudgetService(IBudgetRepo budgetRepo)
{
    public decimal Query(DateTime startTime, DateTime endTime)
    {
        if (startTime > endTime)
        {
            return 0;
        }

        var budgets = budgetRepo.GetAll().ToDictionary(b => b.YearMonth);
        decimal totalAmount = 0;

        var startMonth = new DateTime(startTime.Year, startTime.Month, 1);
        var endMonth = new DateTime(endTime.Year, endTime.Month, 1);

        var currentMonth = startMonth;
        while (currentMonth <= endMonth)
        {
            var yearMonthStr = currentMonth.ToString("yyyyMM");
            if (budgets.TryGetValue(yearMonthStr, out var budget))
            {
                var daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
                var dailyAmount = (decimal)budget.Amount / daysInMonth;

                var startDay = (currentMonth == startMonth) ? startTime.Day : 1;
                var endDay = (currentMonth == endMonth) ? endTime.Day : daysInMonth;

                var effectiveDays = endDay - startDay + 1;
                totalAmount += dailyAmount * effectiveDays;
            }

            currentMonth = currentMonth.AddMonths(1);
        }

        return totalAmount;
    }
}
