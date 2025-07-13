using System.ComponentModel.DataAnnotations;

namespace BrightBudget.Core.Models
{
    public class SavingGoal
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string GoalName { get; set; } = string.Empty;

        [Required]
        public decimal TargetAmount { get; set; }

        public DateTime? TargetDate { get; set; }

        public string? Note { get; set; }

        [Required]
        public int UserId { get; set; }
        
        public ICollection<SavingTransaction> Transactions { get; set; } = new List<SavingTransaction>();
    }
}