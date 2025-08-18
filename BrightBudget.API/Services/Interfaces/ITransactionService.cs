using BrightBudget.API.Dtos.Transaction;
using BrightBudget.API.Models;

namespace BrightBudget.API.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetByWalletAsync(int walletId);
        Task<IEnumerable<Transaction>> GetByCategoryAsync(int transactionCategoryId);
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime from, DateTime to);
        Task<Transaction> GetByIdAsync(int id);
        Task<IEnumerable<Transaction>> SearchAsync(TransactionQuery query);
        Task CreateAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(int id);
    }
}