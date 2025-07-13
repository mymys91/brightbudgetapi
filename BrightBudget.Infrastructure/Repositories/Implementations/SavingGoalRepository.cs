using BrightBudget.Core.Models;
using BrightBudget.Infrastructure.Data;
using BrightBudget.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrightBudget.Infrastructure.Repositories.Implementations
{
    public class SavingGoalRepository : ISavingGoalRepository
    {
        private readonly AppDbContext _context;

        public SavingGoalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SavingGoal?> GetByIdAsync(int id)
        {
            return await _context.SavingGoals.FindAsync(id);
        }

        public async Task<IEnumerable<SavingGoal>> GetAllAsync()
        {
            return await _context.SavingGoals.ToListAsync();
        }

        public async Task AddAsync(SavingGoal goal)
        {
            await _context.SavingGoals.AddAsync(goal);
        }

        public async Task UpdateAsync(SavingGoal goal)
        {
            _context.SavingGoals.Update(goal);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var goal = await _context.SavingGoals.FindAsync(id);
            if (goal != null)
            {
                _context.SavingGoals.Remove(goal);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
