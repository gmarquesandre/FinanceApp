using FinanceApp.Shared.Dto;

namespace FinanceApp.Core.Services.ForecastServices
{
    public interface IForecastService : ITransientService
    {
        Task<List<ForecastList>> GetForecast();
    }
}