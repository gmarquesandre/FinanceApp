using AutoMapper;
using FinanceApp.Shared.Dto.Spending;
using FinanceApp.Shared.Models.UserTables;

namespace FinanceApp.Api.Profiles
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