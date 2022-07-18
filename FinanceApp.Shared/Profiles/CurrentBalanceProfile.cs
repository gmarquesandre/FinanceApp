using AutoMapper;
using FinanceApp.Shared.Dto.CurrentBalance;
using FinanceApp.Shared.Models.UserTables;

namespace FinanceApp.Shared.Profiles
{
    public class CurrentBalaceProfile : Profile
    {
        public CurrentBalaceProfile()
        {
            CreateMap<CreateOrUpdateCurrentBalance, FGTS>();
            CreateMap<FGTS, CreateOrUpdateCurrentBalance>();
            CreateMap<CurrentBalanceDto, FGTS>();
            CreateMap<FGTS, CurrentBalanceDto>();
        }
    }
}