using System.Collections.Generic;
using System.Threading.Tasks;

using BrightBudget.Core.Models;

namespace BrightBudget.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task DeleteAsync(int userId);
        Task SaveAsync();
    }
}