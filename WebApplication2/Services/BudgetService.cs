using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Services;

public class BudgetService(IBudgetRepo budgetRepo)
{
    public decimal Query(DateTime startTime, DateTime endTime)
    {
        var budgets = budgetRepo.GetAll();
        return Budget.GetTotalAmount(budgets, startTime, endTime);
    }
}
