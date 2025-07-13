using System.Collections.Generic;
using System.Threading.Tasks;

using BrightBudget.Core.Models;
using BrightBudget.Infrastructure.Data;
using BrightBudget.Infrastructure.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace BrightBudget.Infrastructure.Repositories.Implementations
{
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        private readonly AppDbContext _context;

        public ExpenseCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ExpenseCategory?> GetByIdAsync(int id)
        {
            return await _context.ExpenseCategories.FindAsync(id);
        }

        public async Task<IEnumerable<ExpenseCategory>> GetAllAsync()
        {
            return await _context.ExpenseCategories.ToListAsync();
        }

        public async Task AddAsync(ExpenseCategory category)
        {
            await _context.ExpenseCategories.AddAsync(category);
        }

        public async Task UpdateAsync(ExpenseCategory category)
        {
            _context.ExpenseCategories.Update(category);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.ExpenseCategories.FindAsync(id);
            if (category != null)
            {
                _context.ExpenseCategories.Remove(category);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}