namespace BrightBudget.API.Dtos.Transaction
{
    public class TransactionReadDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public int WalletId { get; set; }
    }
}
