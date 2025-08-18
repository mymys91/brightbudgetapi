using BrightBudget.API.Enums;

namespace BrightBudget.API.Dtos.Transaction
{
    public class TransactionCreateDto
    {
        public int WalletId { get; set; }
        public int TransactionCategoryId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public TransactionType Type { get; set; } = TransactionType.Expense;
    }
}