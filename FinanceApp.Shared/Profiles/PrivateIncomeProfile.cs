using AutoMapper;
using FinanceApp.Shared.Dto.PrivateFixedInvestment;
using FinanceApp.Shared.Models.UserTables;

namespace FinanceApp.Api.Profiles
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