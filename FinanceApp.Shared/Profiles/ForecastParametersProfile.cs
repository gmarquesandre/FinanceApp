using AutoMapper;
using FinanceApp.Shared.Dto.CurrentBalance;
using FinanceApp.Shared.Models.UserTables;

namespace FinanceApp.Shared.Profiles
{
    public class ForecastParametersProfile : Profile
    {
        public ForecastParametersProfile()
        {
            CreateMap<CreateOrUpdateCurrentBalance, ForecastParameters>();
            CreateMap<ForecastParameters, CreateOrUpdateCurrentBalance>();
            CreateMap<CurrentBalanceDto, ForecastParameters>();
            CreateMap<ForecastParameters, CurrentBalanceDto>();
        }
    }
}