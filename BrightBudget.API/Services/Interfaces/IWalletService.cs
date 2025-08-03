using BrightBudget.API.Dtos.Wallet;
using BrightBudget.API.Models;

namespace BrightBudget.API.Services.Interfaces
{
    public interface IWalletService
    {
        Task<IEnumerable<Wallet>> GetAllAsync();
        Task<Wallet?> GetByIdAsync(int id);
        Task<Wallet> CreateAsync(WalletCreateDto dto, string userId);
        Task<bool> UpdateAsync(int id, WalletUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}