using AutoMapper;
using FinanceApp.Shared.Dto.CurrentBalance;
using FinanceApp.Shared.Dto.FGTS;
using FinanceApp.Shared.Models.UserTables;

namespace FinanceApp.Shared.Profiles
{
    public class FGTSProfile : Profile
    {
        public FGTSProfile()
        {
            CreateMap<CreateOrUpdateCurrentBalance, FGTS>();
            CreateMap<FGTS, CreateOrUpdateCurrentBalance>();
            CreateMap<CurrentBalanceDto, FGTS>();
            CreateMap<FGTS, CurrentBalanceDto>();
        }
    }
}