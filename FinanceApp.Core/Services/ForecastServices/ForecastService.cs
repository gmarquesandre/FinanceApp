using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models.CommonTables;

namespace FinanceApp.Core.Services.ForecastServices
{
    public class ForecastService : IForecastService
    {
        public ISpendingService _spendingService;
        public ForecastService(ISpendingService spendingService)
        {
            _spendingService = spendingService;
        }

        public async Task<ForecastList> GetForecast(CustomIdentityUser user)
        {
            var spendingsDaily = await _spendingService.GetForecast(user);

            return value;
        }

    }
}
