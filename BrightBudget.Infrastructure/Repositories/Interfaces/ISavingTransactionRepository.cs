using BrightBudget.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrightBudget.Infrastructure.Repositories.Interfaces
{
    public interface ISavingTransactionRepository
    {
        Task<SavingTransaction?> GetByIdAsync(int id);
        Task<IEnumerable<SavingTransaction>> GetAllAsync();
        Task AddAsync(SavingTransaction transaction);
        Task UpdateAsync(SavingTransaction transaction);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
