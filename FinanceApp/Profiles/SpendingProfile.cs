using AutoMapper;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;

namespace UsuariosApi.Profiles
{
    public class SpendingProfile : Profile
    {
        public SpendingProfile()
        {
            CreateMap<CreateSpending, Spending>();
            CreateMap<UpdateSpending, Spending>();
            CreateMap<SpendingDto, Spending>();
            CreateMap<Spending, SpendingDto>();
            CreateMap<UpdateSpending, Spending>();
        }
    }
}