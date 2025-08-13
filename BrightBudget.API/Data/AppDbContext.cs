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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WalletType>().HasData(
                new WalletType { Id = 101, Name = "Checking" },
                new WalletType { Id = 102, Name = "Savings" },
                new WalletType { Id = 103, Name = "Credit" },
                new WalletType { Id = 104, Name = "Investment" }
            );
            modelBuilder.Entity<Wallet>()
                   .Property(w => w.CurrencyCode)
                   .HasConversion<string>();
        }
    }
}