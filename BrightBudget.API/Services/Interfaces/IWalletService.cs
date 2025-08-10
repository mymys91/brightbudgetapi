using BrightBudget.API.Dtos.Wallet;
using BrightBudget.API.Models;

namespace BrightBudget.API.Services.Interfaces
{
    public interface IWalletService
    {
        Task<IEnumerable<Wallet>> GetAllAsync();
        Task<Wallet?> GetByIdAsync(int id);
        Task<IEnumerable<Wallet>> GetByUserIdAsync(string userId);
        Task<Wallet> CreateAsync(WalletCreateDto dto, string userId);
        Task<bool> UpdateAsync(int id, WalletUpdateDto dto, string userId);
        Task<bool> DeleteAsync(int id, string userId);
    }
}