using System.ComponentModel.DataAnnotations;

namespace BrightBudget.Core.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
               
        public ICollection<Expense>? Expenses { get; set; }
        public ICollection<ExpenseCategory>? Categories { get; set; }
        public ICollection<SavingGoal>? SavingGoals { get; set; }
    }
}