using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;
using static FinanceApp.Core.Services.TitleService;

namespace FinanceApp.Core.Services.ForecastServices
{
    public class ForecastService : IForecastService
    {
        public ISpendingService _spendingService;
        public IIncomeService _incomeService;
        public ILoanService _loanService;
        public ICurrentBalanceService _currentBalanceService;
        public IIndexService _indexService;
        public ITitleService _titleService;

        public ForecastService(ISpendingService spendingService,
            IIncomeService incomeService,
            ICurrentBalanceService currentBalanceService,
            ILoanService loanService,
            IIndexService indexService,
            ITitleService titleService)
        {
            _spendingService = spendingService;
            _incomeService = incomeService;
            _currentBalanceService = currentBalanceService;
            _loanService = loanService;
            _indexService = indexService;
            _titleService = titleService;
        }

        public async Task<List<ForecastList>> GetForecast(CustomIdentityUser user)
        {
            DateTime maxYearMonth = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1).AddMonths(13).AddDays(-1);

            var spendingsDaily = await _spendingService.GetForecast(user, EForecastType.Daily, maxYearMonth);
            var incomesDaily = await _incomeService.GetForecast(user, EForecastType.Daily, maxYearMonth);
            var loanDaily = await _loanService.GetForecast(user, EForecastType.Daily, maxYearMonth);

            var balance = await _currentBalanceService.GetAsync(user);

            var balanceTitlesList = new List<DefaultTitleInput>();

            bool updateValueWithCdiIndex = balance.UpdateValueWithCdiIndex;

            double percentageCdi = balance.PercentageCdi ?? 1.00;

            if (balance != null)
            {
                balanceTitlesList.Add(new DefaultTitleInput()
                {
                    DateInvestment = balance.UpdateDateTime ?? balance.CreationTime,
                    InvestmentValue = balance.Value,
                    IndexPercentage = balance.PercentageCdi ?? 0.00,
                    Index = EIndex.CDI,
                    AdditionalFixedInterest = 0.00,
                    TypePrivateFixedIncome = ETypePrivateFixedIncome.BalanceCDB
                });
            }


            for (DateTime date = DateTime.Now.Date; date <= maxYearMonth; date = date.AddMonths(1))
            {

                while (date.Day > 1)
                {

                    double loansDay = 0.00;
                    double incomesDay = 0.00;
                    double spendingsDay = 0.00;

                    //variavel responsável por avisar se há alterações no balanço
                    bool updateBalance = false;

                    if (incomesDaily.Items.Any(a => a.DateReference == date))
                    {
                        incomesDay = incomesDaily.Items.Where(a => a.DateReference == date).Sum(a => a.Amount);
                        updateBalance = true;
                    }
                    if (spendingsDaily.Items.Any(a => a.DateReference == date))
                    {
                        spendingsDay = incomesDaily.Items.Where(a => a.DateReference == date).Sum(a => a.Amount);
                        updateBalance = true;
                    }
                    if (loanDaily.Items.Any(a => a.DateReference == date))
                    {
                        loansDay = incomesDaily.Items.Where(a => a.DateReference == date).Sum(a => a.Amount);
                        updateBalance = true;
                    }

                    if (updateBalance)
                    {
                        //atualizar saldo inicial de titulos liquidos pelo indice
                        if (incomesDay >= loansDay + spendingsDay)
                        {
                            if (incomesDay > loansDay + spendingsDay)
                            {
                                balanceTitlesList.Add(new DefaultTitleInput()
                                {
                                    DateInvestment = date,
                                    InvestmentValue = incomesDay,
                                    IndexPercentage = balance.PercentageCdi ?? 0.00,
                                    Index = EIndex.CDI,
                                    AdditionalFixedInterest = 0.00,
                                    TypePrivateFixedIncome = ETypePrivateFixedIncome.BalanceCDB
                                });
                            }

                        }
                        else if (incomesDay < loansDay + spendingsDay)
                        {
                            double leftValue = spendingsDay + loansDay - incomesDay;

                            double totalSpendingDayValue = Convert.ToDouble(spendingsDay + loansDay);

                            balanceTitlesList.ForEach(async title =>
                                {
                                    if (totalSpendingDayValue <= 0.00)
                                        return;

                                    title.Date = date;                                    
                                    var (titleOutput, withdraw) = await _titleService.GetCurrentTitleAfterWithdraw(title, totalSpendingDayValue);

                                    var titleUpdated = titleOutput;
                                    var withdrawTotal = withdraw;

                                    if(titleUpdated.LiquidValue > 0.00)
                                    {
                                        totalSpendingDayValue = 0.00;
                                        return ;
                                    }
                                    else if(titleUpdated.LiquidValue == 0.00)
                                    {
                                        totalSpendingDayValue -= withdrawTotal;
                                        balanceTitlesList.Remove(title);
                                    }                                   
                                }
                            );


                            if (totalSpendingDayValue > 0)
                            {

                            }



                        }
                    }


                    date = date.AddDays(1);
                }



            }



            return null;
        }
    }
}
