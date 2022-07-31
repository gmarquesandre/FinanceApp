using AutoMapper;
using FinanceApp.Shared.Dto.ForecastParameters;
using FinanceApp.Shared.Entities.UserTables;

namespace FinanceApp.Shared.Profiles
{
    public class ForecastParametersProfile : Profile
    {
        public ForecastParametersProfile()
        {
            CreateMap<CreateOrUpdateForecastParameters, ForecastParameters>();
            CreateMap<ForecastParameters, CreateOrUpdateForecastParameters>();
            CreateMap<ForecastParametersDto, ForecastParameters>();
            CreateMap<ForecastParameters, ForecastParametersDto>();
        }
    }
}