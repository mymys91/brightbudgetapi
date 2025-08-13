using System.ComponentModel.DataAnnotations;

namespace BrightBudget.API.Models
{
    public class WalletType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}