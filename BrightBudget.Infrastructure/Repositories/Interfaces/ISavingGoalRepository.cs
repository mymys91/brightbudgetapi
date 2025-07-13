using System.Collections.Generic;
using System.Threading.Tasks;

using BrightBudget.Core.Models;

namespace BrightBudget.Infrastructure.Repositories.Interfaces
{
    public interface ISavingGoalRepository
    {
        Task<SavingGoal?> GetByIdAsync(int id);
        Task<IEnumerable<SavingGoal>> GetAllAsync();
        Task AddAsync(SavingGoal goal);
        Task UpdateAsync(SavingGoal goal);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}