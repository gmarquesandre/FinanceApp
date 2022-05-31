using AutoMapper;
using FinanceApp.Shared.Dto.CurrentBalance;
using FinanceApp.Shared.Models.UserTables;

namespace FinanceApp.Api.Profiles
{
    public class CurrentBalanceProfile : Profile
    {
        public CurrentBalanceProfile()
        {
            CreateMap<CreateOrUpdateCurrentBalance, CurrentBalance>();
            CreateMap<CurrentBalance, CreateOrUpdateCurrentBalance>();
            CreateMap<CurrentBalanceDto, CurrentBalance>();
            CreateMap<CurrentBalance, CurrentBalanceDto>();
        }
    }
}