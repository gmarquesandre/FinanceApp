using AutoMapper;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;

namespace UsuariosApi.Profiles
{
    public class IncomeProfile : Profile
    {
        public IncomeProfile()
        {
            CreateMap<CreateIncome, Income>();
            CreateMap<UpdateIncome, Income>();
            CreateMap<IncomeDto, Income>();
            CreateMap<Income, IncomeDto>();
            CreateMap<UpdateIncome, Income>();
        }
    }
}