using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models.CommonTables;

namespace FinanceApp.Core.Services.CrudServices
{
    public interface IForecastService
    {
        Task<List<ForecastItem>> GetForecast(CustomIdentityUser user);
    }
}