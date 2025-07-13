using Microsoft.EntityFrameworkCore;
using BrightBudget.Core.Models;

namespace BrightBudget.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<ExpenseCategory> ExpenseCategories => Set<ExpenseCategory>();
        public DbSet<SavingGoal> SavingGoals => Set<SavingGoal>();
        public DbSet<SavingTransaction> SavingTransactions => Set<SavingTransaction>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}