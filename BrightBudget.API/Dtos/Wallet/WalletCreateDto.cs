using System.ComponentModel.DataAnnotations;
using BrightBudget.API.Enums;

namespace BrightBudget.API.Dtos.Wallet
{
    public class WalletCreateDto
    {
        [Required(ErrorMessage = "Wallet name is required")]
        public string Name { get; set; } = null!;
        public int WalletTypeId { get; set; }
        public decimal InitialBalance { get; set; } = 0;
        public CurrencyCode CurrencyCode { get; set; } = CurrencyCode.VND;
    }
}