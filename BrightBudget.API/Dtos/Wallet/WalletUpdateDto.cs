namespace BrightBudget.API.Dtos.Wallet
{
    public class WalletUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Currency { get; set; } = "VND";
    }
}