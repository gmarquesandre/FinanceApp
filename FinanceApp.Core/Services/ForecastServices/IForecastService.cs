using FinanceApp.Shared;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices
{
    public interface IForecastService 
    {
        Task<List<ForecastList>> GetForecast(DateTime currentDate, DateTime lastDate, EForecastType forecastType, bool forceUpdate);
    }
}