using AutoMapper;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;

namespace UsuariosApi.Profiles
{
    public class FixedInterestInvestmentProfile : Profile
    {
        public FixedInterestInvestmentProfile()
        {
            CreateMap<FixedInterestInvestment, FixedInterestInvestmentDto>();
            CreateMap<FixedInterestInvestmentDto, FixedInterestInvestment>();
        }
    }
}