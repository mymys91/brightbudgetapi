using System.ComponentModel.DataAnnotations;

namespace BrightBudget.API.Dtos.Wallet
{
    public class WalletCreateDto
    {
        [Required(ErrorMessage = "Wallet name is required")]
        public string Name { get; set; } = null!;
        public decimal InitialBalance { get; set; }
        public string Currency { get; set; } = "VND";
    }
}