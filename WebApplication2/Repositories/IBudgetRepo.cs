using WebApplication2.Models;

namespace WebApplication2.Repositories;

public interface IBudgetRepo
{
    List<Budget> GetAll();
}