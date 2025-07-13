using System.Collections.Generic;
using System.Threading.Tasks;

using BrightBudget.Core.Models;

namespace BrightBudget.Infrastructure.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<Expense>> GetByUserAsync(int userId);
        Task AddAsync(Expense expense);
        Task UpdateAsync(Expense expense);
        Task DeleteAsync(int expenseId);
        Task SaveAsync();
    }
}