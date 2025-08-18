using AutoMapper;
using BrightBudget.API.Dtos.Wallet;
using BrightBudget.API.Models;

namespace BrightBudget.API.Mappings
{
    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            CreateMap<Wallet, WalletReadDto>()
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode.ToString()))
               .ForMember(dest => dest.WalletTypeName, opt => opt.MapFrom(src => src.WalletType.Name));
            CreateMap<WalletCreateDto, Wallet>();
            CreateMap<WalletUpdateDto, Wallet>();
        }
    }
}
