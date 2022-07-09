using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models.CommonTables;

namespace FinanceApp.Core.Services.ForecastServices
{
    public interface IForecastService
    {
        Task<List<ForecastList>> GetForecast();
    }
}