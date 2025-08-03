using AutoMapper;
using BrightBudget.API.Dtos.Wallet;
using BrightBudget.API.Models;

namespace BrightBudget.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Wallet, WalletReadDto>();
            CreateMap<WalletCreateDto, Wallet>();
            CreateMap<WalletUpdateDto, Wallet>();
        }
    }
}
