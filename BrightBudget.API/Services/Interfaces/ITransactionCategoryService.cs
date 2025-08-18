using BrightBudget.API.Dtos.TransactionCategory;

namespace BrightBudget.API.Services.Interfaces
{
    public interface ITransactionCategoryService
    {
        Task<IEnumerable<TransactionCategoryReadDto>> GetAllAsync(string userId);
        Task<TransactionCategoryReadDto?> GetByIdAsync(int id, string userId);
        Task<TransactionCategoryReadDto> CreateAsync(TransactionCategoryCreateDto dto, string userId);
        Task<bool> DeleteAsync(int id, string userId);
    }
}