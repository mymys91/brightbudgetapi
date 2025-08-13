
using AutoMapper;
using BrightBudget.API.Data;
using BrightBudget.API.Dtos.Transaction;
using BrightBudget.API.Enums;
using BrightBudget.API.Models;
using BrightBudget.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrightBudget.API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public TransactionService(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Transaction> CreateTransactionAsync(TransactionCreateDto dto)
        {
            var wallet = await _context.Wallets.FindAsync(dto.WalletId);
            if (wallet == null) throw new Exception("Wallet not found");

            var transaction = _mapper.Map<Transaction>(dto);
            _context.Transactions.Add(transaction);

            wallet.Balance += dto.Type == TransactionType.Income ? dto.Amount : -dto.Amount;
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync()
        {
            return await _context.Transactions.ToListAsync();
        }
    }
}