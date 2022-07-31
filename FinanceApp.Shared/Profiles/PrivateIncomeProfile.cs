using AutoMapper;
using FinanceApp.Shared.Dto.PrivateFixedInvestment;
using FinanceApp.Shared.Entities.UserTables;

namespace FinanceApp.Shared.Profiles
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