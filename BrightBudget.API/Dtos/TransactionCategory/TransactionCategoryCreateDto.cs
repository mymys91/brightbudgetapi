using System.ComponentModel.DataAnnotations;

namespace BrightBudget.API.Dtos.TransactionCategory
{
    public class TransactionCategoryCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}