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
       
        public async Task<WalletReadDto?> GetByIdAsync(int id, string userId)
        {
           var wallet = await _context.Wallets
                            .Include(w => w.WalletType)
                            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);
            if (wallet == null)
                return null;
            return  _mapper.Map<WalletReadDto>(wallet);
        }

        public async Task<IEnumerable<WalletReadDto>> GetByUserIdAsync(string userId)
        {
            var wallets = await _context.Wallets
                .Include(w => w.WalletType)
                .Where(w => w.UserId == userId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<WalletReadDto>>(wallets);
        }

        public async Task<WalletReadDto> CreateAsync(WalletCreateDto dto, string userId)
        {
            var wallet = _mapper.Map<Wallet>(dto);
            wallet.UserId = userId;

            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
            wallet.WalletType = await _context.WalletTypes.FirstAsync(t => t.Id == wallet.WalletTypeId);

            return _mapper.Map<WalletReadDto>(wallet);;
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
