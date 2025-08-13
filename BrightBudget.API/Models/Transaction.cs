
using BrightBudget.API.Enums;

namespace BrightBudget.API.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public Wallet Wallet { get; set; } = null!; // Navigation property
    }
}