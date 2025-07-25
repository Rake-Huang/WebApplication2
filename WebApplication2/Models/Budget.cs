namespace WebApplication2.Models;

public class Budget
{
    public string YearMonth { get; set; }
    public int Amount { get; set; }

    public decimal GetDailyAmount()
    {
        if (!DateTime.TryParseExact(YearMonth + "01", "yyyyMMdd", null, 
            System.Globalization.DateTimeStyles.None, out var monthDate))
            return 0;

        var daysInMonth = DateTime.DaysInMonth(monthDate.Year, monthDate.Month);
        return (decimal)Amount / daysInMonth;
    }

    public static decimal GetTotalAmount(IEnumerable<Budget> budgets, DateTime startTime, DateTime endTime)
    {
        if (startTime > endTime)
            return 0;

        var budgetDict = budgets.ToDictionary(b => b.YearMonth);
        var periods = Period.CreateMonthlyPeriods(startTime, endTime);
        
        decimal totalAmount = 0;
        
        foreach (var period in periods)
        {
            var yearMonth = period.StartDate.ToString("yyyyMM");
            if (budgetDict.TryGetValue(yearMonth, out var budget))
            {
                totalAmount += budget.GetDailyAmount() * period.OverLappingDays;
            }
        }

        return totalAmount;
    }
}