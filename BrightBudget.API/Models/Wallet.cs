using System.ComponentModel.DataAnnotations;
using BrightBudget.API.Enums;

namespace BrightBudget.API.Models
{
    public class Wallet
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public int WalletTypeId { get; set; }
        public WalletType WalletType { get; set; } = null!;

        public decimal Balance { get; set; }

        public CurrencyCode CurrencyCode { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser? User { get; set; }
    }
}