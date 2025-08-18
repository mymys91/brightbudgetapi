using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BrightBudget.API.Models;

namespace BrightBudget.API.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Wallet> Wallets { get; set; } = null!;
        public DbSet<WalletType> WalletTypes { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<TransactionCategory> TransactionCategories { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WalletType>().HasData(
                new WalletType { Id = 101, Name = "Checking" },
                new WalletType { Id = 102, Name = "Savings" },
                new WalletType { Id = 103, Name = "Credit" },
                new WalletType { Id = 104, Name = "Investment" });
            modelBuilder.Entity<TransactionCategory>().HasData(
                new TransactionCategory { Id = 1, Name = "Food & Dining", UserId = null },
                new TransactionCategory { Id = 2, Name = "Transportation", UserId = null },
                new TransactionCategory { Id = 3, Name = "Entertainment", UserId = null },
                new TransactionCategory { Id = 4, Name = "Savings", UserId = null },
                new TransactionCategory { Id = 5, Name = "Utilities", UserId = null },
                new TransactionCategory { Id = 6, Name = "Health & Fitness", UserId = null },
                new TransactionCategory { Id = 7, Name = "Shopping", UserId = null },
                new TransactionCategory { Id = 8, Name = "Education", UserId = null },
                new TransactionCategory { Id = 9, Name = "Travel", UserId = null },
                new TransactionCategory { Id = 10, Name = "Others", UserId = null });
            modelBuilder.Entity<Wallet>()
                   .Property(w => w.CurrencyCode)
                   .HasConversion<string>();
        }
    }
}