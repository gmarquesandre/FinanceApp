using AutoMapper;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;

namespace UsuariosApi.Profiles
{
    public class PrivateFixedIncomeProfile : Profile
    {
        public PrivateFixedIncomeProfile()
        {
            CreateMap<CreatePrivateFixedIncome, PrivateFixedIncome>();
            CreateMap<UpdatePrivateFixedIncome, PrivateFixedIncome>();
            CreateMap<PrivateFixedIncomeDto, PrivateFixedIncome>();
            CreateMap<PrivateFixedIncome, PrivateFixedIncomeDto>();
            CreateMap<UpdatePrivateFixedIncome, PrivateFixedIncome>();
        }
    }
}