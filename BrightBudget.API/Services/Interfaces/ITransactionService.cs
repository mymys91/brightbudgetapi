using BrightBudget.API.Dtos.Transaction;
using BrightBudget.API.Models;

namespace BrightBudget.API.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> CreateTransactionAsync(TransactionCreateDto dto);
        Task<IEnumerable<Transaction>> GetTransactionsAsync();
    }
}