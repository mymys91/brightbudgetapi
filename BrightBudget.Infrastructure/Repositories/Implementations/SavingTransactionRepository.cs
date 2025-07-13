using System.Collections.Generic;
using System.Threading.Tasks;

using BrightBudget.Core.Models;
using BrightBudget.Infrastructure.Data;
using BrightBudget.Infrastructure.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace BrightBudget.Infrastructure.Repositories.Implementations
{
    public class SavingTransactionRepository : ISavingTransactionRepository
    {
        private readonly AppDbContext _context;

        public SavingTransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SavingTransaction?> GetByIdAsync(int id)
        {
            return await _context.SavingTransactions.FindAsync(id);
        }

        public async Task<IEnumerable<SavingTransaction>> GetAllAsync()
        {
            return await _context.SavingTransactions.ToListAsync();
        }

        public async Task AddAsync(SavingTransaction transaction)
        {
            await _context.SavingTransactions.AddAsync(transaction);
        }

        public async Task UpdateAsync(SavingTransaction transaction)
        {
            _context.SavingTransactions.Update(transaction);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var transaction = await _context.SavingTransactions.FindAsync(id);
            if (transaction != null)
            {
                _context.SavingTransactions.Remove(transaction);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}