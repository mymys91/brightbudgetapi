using BrightBudget.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
