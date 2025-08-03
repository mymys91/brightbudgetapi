using System.ComponentModel.DataAnnotations;

namespace BrightBudget.API.Models
{
    public class Wallet
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public decimal InitialBalance { get; set; }

        public string Currency { get; set; } = "VND";

        public string UserId { get; set; } = null!;
        public ApplicationUser? User { get; set; }
    }
}