namespace BrightBudget.API.Dtos.Wallet
{
    public class WalletReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int WalletTypeId { get; set; }
        public string WalletTypeName { get; set; } = null!;
        public decimal Balance { get; set; }
        public string CurrencyCode { get; set; } = null!;
    }
}