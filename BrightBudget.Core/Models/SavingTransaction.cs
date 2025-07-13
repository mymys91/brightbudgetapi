using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrightBudget.Core.Models
{
    public class SavingTransaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Note { get; set; }

        [Required]
        public int SavingGoalId { get; set; }

        [ForeignKey("SavingGoalId")]
        public SavingGoal? SavingGoal { get; set; }
    }
}