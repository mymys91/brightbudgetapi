namespace BrightBudget.API.Dtos.Wallet
{
    public class WalletReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal InitialBalance { get; set; }
        public string Currency { get; set; } = null!;
    }
}