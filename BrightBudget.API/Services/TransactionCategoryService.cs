
using AutoMapper;
using BrightBudget.API.Data;
using BrightBudget.API.Dtos.TransactionCategory;
using BrightBudget.API.Models;
using BrightBudget.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrightBudget.API.Services
{
    public class TransactionCategoryService : ITransactionCategoryService
    {
         private readonly IMapper _mapper;
        private readonly AppDbContext _context;
       
        public TransactionCategoryService(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<TransactionCategoryReadDto>> GetAllAsync(string userId)
        {
            var categories = await _context.TransactionCategories.Where(t=> t.UserId == null || t.UserId == userId).ToListAsync();
            return _mapper.Map<IEnumerable<TransactionCategoryReadDto>>(categories);
        }

        public async Task<TransactionCategoryReadDto?> GetByIdAsync(int id, string userId)
        {
            var category = await _context.TransactionCategories.FindAsync(id);
            if (category == null || category.UserId != userId)
                return null;
            return _mapper.Map<TransactionCategoryReadDto>(category);
        }

        public async Task<TransactionCategoryReadDto> CreateAsync(TransactionCategoryCreateDto dto, string userId)
        {
            var category = _mapper.Map<TransactionCategory>(dto);
            category.UserId = userId;

            _context.TransactionCategories.Add(category);
            await _context.SaveChangesAsync();
            return _mapper.Map<TransactionCategoryReadDto>(category);
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var category = await _context.TransactionCategories.FindAsync(id);
            if (category == null || category.UserId == null || category.UserId != userId) return false;

            _context.TransactionCategories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}