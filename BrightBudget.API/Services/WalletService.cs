using BrightBudget.API.Dtos.Wallet;
using BrightBudget.API.Models;
using BrightBudget.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using BrightBudget.API.Data;
using AutoMapper;

namespace BrightBudget.API.Services
{
    public class WalletService : IWalletService
    {
        private readonly IMapper _mapper;

        private readonly AppDbContext _context;

        public WalletService(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<Wallet>> GetAllAsync()
        {
            return await _context.Wallets
                .Include(w => w.User)
                .ToListAsync();
        }

        public async Task<Wallet?> GetByIdAsync(int id)
        {
            return await _context.Wallets
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<Wallet>> GetByUserIdAsync(string userId)
        {
            return await _context.Wallets
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<Wallet> CreateAsync(WalletCreateDto dto, string userId)
        {
            var wallet = _mapper.Map<Wallet>(dto);
            wallet.UserId = userId;

            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();

            return wallet;
        }

        public async Task<bool> UpdateAsync(int id, WalletUpdateDto dto, string userId)
        {
            var wallet = await _context.Wallets
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

            if (wallet == null)
                return false;

            wallet.Name = dto.Name;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var wallet = await _context.Wallets
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

            if (wallet == null)
                return false;

            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
