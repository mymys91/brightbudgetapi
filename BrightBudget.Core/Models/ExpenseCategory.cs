using System.ComponentModel.DataAnnotations;

namespace BrightBudget.Core.Models
{
    public class ExpenseCategory
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]    
        public int? UserId { get; set; }

        public ICollection<Expense>? Expenses { get; set; }
    }
}