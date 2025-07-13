using System.Collections.Generic;
using System.Threading.Tasks;

using BrightBudget.Core.Models;

namespace BrightBudget.Infrastructure.Repositories.Interfaces
{
    public interface IExpenseCategoryRepository
    {
        Task<ExpenseCategory?> GetByIdAsync(int id);
        Task<IEnumerable<ExpenseCategory>> GetAllAsync();
        Task AddAsync(ExpenseCategory category);
        Task UpdateAsync(ExpenseCategory category);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}