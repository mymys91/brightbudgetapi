namespace BrightBudget.API.Dtos.TransactionCategory
{
    public class TransactionCategoryReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDefault { get; set; }
    }
}