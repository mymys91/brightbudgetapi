using BrightBudget.API.Dtos.Wallet;
using BrightBudget.API.Models;

namespace BrightBudget.API.Services.Interfaces
{
    public interface IWalletService
    {     
        Task<WalletReadDto?> GetByIdAsync(int id, string userId);
        Task<IEnumerable<WalletReadDto>> GetByUserIdAsync(string userId);
        Task<WalletReadDto> CreateAsync(WalletCreateDto dto, string userId);
        Task<bool> UpdateAsync(int id, WalletUpdateDto dto, string userId);
        Task<bool> DeleteAsync(int id, string userId);
    }
}