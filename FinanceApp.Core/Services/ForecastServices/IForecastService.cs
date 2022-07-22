using FinanceApp.Shared.Dto;

namespace FinanceApp.Core.Services.ForecastServices
{
    public interface IForecastService : IScopedService
    {
        Task<List<ForecastList>> GetForecast(DateTime currentDate);
    }
}