using AutoMapper;
using BrightBudget.API.Dtos.Transaction;
using BrightBudget.API.Models;

namespace BrightBudget.API.Mappings
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<TransactionCreateDto, Transaction>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<Transaction, TransactionReadDto>();
        }
    }
}