using AutoMapper;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Models.UserTables;

namespace FinanceApp.Api.Profiles
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