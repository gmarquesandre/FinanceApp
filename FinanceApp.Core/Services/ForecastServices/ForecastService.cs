using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;

namespace FinanceApp.Core.Services.ForecastServices
{
    public class ForecastService : IForecastService
    {
        public ISpendingService _spendingService;
        public IIncomeService _incomeService;
        public ILoanService _loanService;
        public ICurrentBalanceService _currentBalanceService;

        public ForecastService(ISpendingService spendingService, IIncomeService incomeService, ICurrentBalanceService currentBalanceService, ILoanService loanService)
        {
            _spendingService = spendingService;
            _incomeService = incomeService;
            _currentBalanceService = currentBalanceService;
            _loanService = loanService;
        }

        public async Task<List<ForecastList>> GetForecast(CustomIdentityUser user)
        {
            DateTime maxYearMonth = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1).AddMonths(13).AddDays(-1);

            var spendingsDaily = await _spendingService.GetForecast(user, EForecastType.Daily, maxYearMonth);
            var incomesDaily = await _incomeService.GetForecast(user, EForecastType.Daily, maxYearMonth);
            var loanDaily = await _loanService.GetForecast(user, EForecastType.Daily, maxYearMonth);

            var balance = await _currentBalanceService.GetAsync(user);

            decimal balanceInitial = 0.00M;

            if(balance != null)
            {
                balanceInitial = balance.CurrentBalance;
            }


            for (DateTime date = DateTime.Now.Date; date <= maxYearMonth; date = date.AddMonths(1))
            {
                
                while(date.Day > 1)
                {



                    date = date.AddDays(1);
                }
            }
            


            return null;
        }

    }
}
