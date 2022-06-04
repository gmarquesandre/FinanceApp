using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models.CommonTables;

namespace FinanceApp.Core.Services.CrudServices
{
    public class ForecastService : IForecastService
    {
        public ISpendingService _spendingService;
        public ForecastService(ISpendingService spendingService)
        {
            _spendingService = spendingService;
        }

        public async Task<List<ForecastItem>> GetForecast(CustomIdentityUser user)
        {
            var value = await _spendingService.GetForecast(user);

            return value;
        }

    }
}
