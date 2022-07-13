using FinanceApp.Shared.Dto;

namespace FinanceApp.Core.Services.ForecastServices
{
    public interface IForecastService
    {
        Task<List<ForecastList>> GetForecast();
    }
}