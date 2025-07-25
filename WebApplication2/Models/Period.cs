namespace WebApplication2.Models;

public class Period
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    public Period(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public int Days => (EndDate - StartDate).Days + 1;

    public static List<Period> CreateMonthlyPeriods(DateTime startTime, DateTime endTime)
    {
        var periods = new List<Period>();
        
        var startMonth = new DateTime(startTime.Year, startTime.Month, 1);
        var endMonth = new DateTime(endTime.Year, endTime.Month, 1);

        var currentMonth = startMonth;
        while (currentMonth <= endMonth)
        {
            var daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
            var monthEnd = new DateTime(currentMonth.Year, currentMonth.Month, daysInMonth);

            var periodStart = (currentMonth == startMonth) ? startTime : currentMonth;
            var periodEnd = (currentMonth == endMonth) ? endTime : monthEnd;

            periods.Add(new Period(periodStart, periodEnd));
            currentMonth = currentMonth.AddMonths(1);
        }

        return periods;
    }
} 