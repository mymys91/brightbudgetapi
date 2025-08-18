using System.ComponentModel.DataAnnotations;

namespace BrightBudget.API.Models
{
    public class TransactionCategory
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
        public string? UserId { get; set; }
    }
}