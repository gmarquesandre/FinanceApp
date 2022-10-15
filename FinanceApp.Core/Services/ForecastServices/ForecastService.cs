using FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces;
using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Interfaces;
using FinanceApp.FinanceData.Services;
using FinanceApp.Shared;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.ForecastParameters;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices
{
    public class ForecastService : IForecastService
    {
        public ISpendingService _spendingService;
        public IIncomeService _incomeService;
        public ILoanService _loanService;
        public ICurrentBalanceService _currentBalanceService;
        public IIndexService _indexService;
        public IFGTSService _fgtsService;
        public ITitleService _titleService;
        public IForecastParametersService _forecastParametersService;

        public ForecastService(ISpendingService spendingService,
            IIncomeService incomeService,
            ICurrentBalanceService currentBalanceService,
            ILoanService loanService,
            IIndexService indexService,
            ITitleService titleService,
            IFGTSService fgtsService,
            IForecastParametersService forecastParametersService
            )
        {
            _spendingService = spendingService;
            _incomeService = incomeService;
            _currentBalanceService = currentBalanceService;
            _loanService = loanService;
            _indexService = indexService;
            _titleService = titleService;
            _fgtsService = fgtsService;
            _forecastParametersService = forecastParametersService;
        }

        public async Task<List<ForecastList>> GetForecast(DateTime startDate, DateTime lastDate, EForecastType forecastType, bool forceUpdate)
        {

            if (forecastType == EForecastType.Monthly)
                lastDate = lastDate.GetLastDayOfThisMonth();
           
            var output = await GetForecastList(forecastType, startDate.AddDays(1), lastDate, forceUpdate);

            return output;      
        }

        private async Task<List<ForecastList>> GetForecastList(EForecastType forecastType, DateTime startDate, DateTime lastDate, bool forceUpdate)
        {
            
            List<DefaultTitleInput> balanceTitlesList = await GetInitialBalanceAsync();

            var spendingsDaily = await _spendingService.GetForecast(EForecastType.Daily, lastDate, startDate);
            var incomesDaily = await _incomeService.GetForecast(EForecastType.Daily, lastDate, startDate);
            var loanDaily = await _loanService.GetForecast(EForecastType.Daily, lastDate, startDate);
            var fgtsDaily = await _fgtsService.GetForecast(EForecastType.Daily, lastDate, startDate);

            var forecastTotalList = new List<ForecastItem>();

            for (DateTime date = startDate; date <= lastDate; date = date.AddDays(1))
            {
                balanceTitlesList = await DayMovimentation(forceUpdate, spendingsDaily, incomesDaily, loanDaily, fgtsDaily, balanceTitlesList, date);

                if (date.AddDays(1).Day == 1 || forecastType == EForecastType.Daily)
                {
                    forecastTotalList.Add(await AddDayForecast(fgtsDaily, balanceTitlesList, date));
                }
            }

            var totalDaily = new ForecastList()
            {
                Items = forecastTotalList,
                Type = EItemType.Total
            };

            
            if (forecastType == EForecastType.Daily)
            {
                var output = new List<ForecastList>()
                {
                    totalDaily,
                    AddMissingDates(incomesDaily, startDate,lastDate),
                    AddMissingDates(spendingsDaily, startDate,lastDate),
                    AddMissingDates(loanDaily, startDate,lastDate),
                    AddMissingDates(fgtsDaily,startDate ,lastDate)
                };

                return output;
            }
            else if (forecastType == EForecastType.Monthly)
            {
                var output = new List<ForecastList>()
                {
                    totalDaily,
                    GroupMonthly(incomesDaily, startDate, lastDate),
                    GroupMonthly(spendingsDaily, startDate, lastDate),
                    GroupMonthly(loanDaily, startDate, lastDate),
                    GroupMonthly(fgtsDaily, startDate, lastDate)
                };

                return output;
            }

            throw new Exception("Período inválido");
        }

        private async Task<List<DefaultTitleInput>> GetInitialBalanceAsync()
        {
            var balance = await _currentBalanceService.GetAsync();

            var balanceTitlesList = new List<DefaultTitleInput>();

            if (balance != null && balance.Value > 0.00)
            {
                balanceTitlesList.Add(new DefaultTitleInput()
                {
                    DateInvestment = balance.UpdateDateTime,
                    InvestmentValue = balance.Value,
                    IndexPercentage = balance.UpdateValueWithCdiIndex ? balance.PercentageCdi ?? 0.00 : 0,
                    Index = EIndex.CDI,
                    AdditionalFixedInterest = 0.00,                    
                    TypePrivateFixedIncome = ETypePrivateFixedIncome.BalanceCDB
                });
            }

            return balanceTitlesList;
        }

        private async Task<ForecastItem> AddDayForecast( ForecastList fgtsDaily, List<DefaultTitleInput> balanceTitlesList, DateTime date)
        {
            var valorTitulos = balanceTitlesList.Select(async a =>
            {
                return await _titleService.GetCurrentValueOfTitle(a, date);
            }).Select(a => a.Result).Sum(a => a.LiquidValue);

            double valorTitulosReal = await _indexService.GetRealValue(date, valorTitulos);

            double notLiquidNominal = fgtsDaily.Items.Where(a => a.DateReference == date).Sum(a => a.NominalCumulatedAmount);

            double notLiquidReal = await _indexService.GetRealValue(date, notLiquidNominal);

            return new ForecastItem()
            {
                RealLiquidValue = valorTitulosReal,
                NominalLiquidValue = valorTitulos,
                NominalCumulatedAmount = valorTitulos,
                NominalNotLiquidValue = notLiquidNominal,
                RealCumulatedAmount = notLiquidReal,
                DateReference = date,
            };
        }

        private async Task<List<DefaultTitleInput>> DayMovimentation(bool forceUpdate, ForecastList spendingsDaily, ForecastList incomesDaily, ForecastList loanDaily, ForecastList fgtsDaily, List<DefaultTitleInput> balanceTitlesList, DateTime date)
        {
            var forecastParameters = await _forecastParametersService.GetAsync();

            DayMovimentation dayMovimentation = CheckIfMustUpdateBalance(spendingsDaily, incomesDaily, loanDaily, fgtsDaily, date);

            if (dayMovimentation.UpdateBalance || forceUpdate)
            {
                if (balanceTitlesList.Any(a => a.InvestmentValue < 0.00))
                {
                    balanceTitlesList = AddOwingValue(balanceTitlesList, date, dayMovimentation);
                }

                if (dayMovimentation.PositiveBalanceDay)
                {
                    balanceTitlesList = AddNewDeposit(forecastParameters, balanceTitlesList, date, dayMovimentation);
                }

                else if (dayMovimentation.NegativeBalanceDay)
                {
                    balanceTitlesList = await WithDrawValuesAsync(balanceTitlesList, dayMovimentation, forecastParameters, date);

                }
                else throw new Exception("Erro ao realizar cálculos");

            }

            return balanceTitlesList;
        }

        private async Task<List<DefaultTitleInput>> WithDrawValuesAsync(List<DefaultTitleInput> balanceTitlesList, DayMovimentation dayMovimentation, ForecastParametersDto forecastParameters, DateTime date)
        {
            double leftValue = -dayMovimentation.ResultValue;

            foreach (var title in balanceTitlesList.Where(a => a.InvestmentValue > 0.00).ToList())
            {
                if (Math.Round(leftValue, 2) <= 0.00)
                    break;

                var (titleOutput, withdraw) = await _titleService.GetCurrentTitleAfterWithdraw(title, date, leftValue);

                var titleUpdated = titleOutput;
                var withdrawTotal = withdraw;

                title.InvestmentValue = titleUpdated.InvestmentValue;

                if (titleUpdated.LiquidValue > 0.00)
                {
                    title.InvestmentValue = titleUpdated.InvestmentValue;
                    leftValue = 0.00;
                    break;
                }
                else if (titleUpdated.LiquidValue == 0.00)
                {
                    leftValue -= withdrawTotal;
                    title.InvestmentValue = 0.00;
                }

            }

            //Remove "Titulos" zerados
            balanceTitlesList = balanceTitlesList.Where(a => Math.Round(a.InvestmentValue, 2) != 0.00).ToList();


            if (leftValue > 0)
            {
                //Remove titulo de divida atual e atualiza
                balanceTitlesList = AddOwingValue(balanceTitlesList, forecastParameters, date, leftValue);
            }

            return balanceTitlesList;
        }

        private static List<DefaultTitleInput> AddOwingValue(List<DefaultTitleInput> balanceTitlesList, ForecastParametersDto forecastParameters, DateTime date, double leftValue)
        {
            balanceTitlesList = balanceTitlesList.Where(a => Math.Round(a.InvestmentValue, 2) > 0.00).ToList();

            balanceTitlesList.Add(new DefaultTitleInput()
            {
                DateInvestment = date,
                InvestmentValue = -leftValue,
                IndexPercentage = forecastParameters.PercentageCdiLoan,
                Index = EIndex.CDI,
                AdditionalFixedInterest = 0.00,
                TypePrivateFixedIncome = ETypePrivateFixedIncome.BalanceCDB
            });
            return balanceTitlesList;
        }

        private static List<DefaultTitleInput> AddNewDeposit(ForecastParametersDto forecastParameters, List<DefaultTitleInput> balanceTitlesList, DateTime date, DayMovimentation dayMovimentation)
        {
            balanceTitlesList = balanceTitlesList.Where(a => a.InvestmentValue > 0.00).ToList();

            double newInvestmentValue = dayMovimentation.ResultValue;
            if(newInvestmentValue > 0.00)
            {
                balanceTitlesList.Add(new DefaultTitleInput()
                {
                    DateInvestment = date,
                    InvestmentValue = Math.Round(newInvestmentValue),
                    IndexPercentage = forecastParameters.PercentageCdiFixedInteresIncometSavings,
                    Index = EIndex.CDI,
                    AdditionalFixedInterest = 0.00,
                    TypePrivateFixedIncome = ETypePrivateFixedIncome.BalanceCDB
                });
            }
            
            return balanceTitlesList;
        }

        private List<DefaultTitleInput> AddOwingValue(List<DefaultTitleInput> balanceTitlesList, DateTime date, DayMovimentation dayMovimentation)
        {
            dayMovimentation.OwingValue = -balanceTitlesList.Where(a => a.InvestmentValue < 0.00).Select(async a =>
            {
                return await _titleService.GetCurrentValueOfTitle(a, date);
            }).Select(a => a.Result).Sum(a => a.LiquidValue);

            return balanceTitlesList;
        }

        private static ForecastList AddMissingDates(ForecastList list, DateTime startDate, DateTime lastDate)
        {
            var allDates = GetAllDatesBetween(startDate, lastDate);

            var listDates = list.Items.Select(a => a.DateReference).ToList();

            var missingDates = allDates.Where(date => !listDates.Contains(date)).ToList();

            missingDates.ForEach(date => list.Items.Add(new ForecastItem()
            {
                DateReference = date,
                NominalCumulatedAmount = 0,
                NominalLiquidValue = 0,
                NominalNotLiquidValue = 0,
                RealCumulatedAmount = 0,
                RealLiquidValue = 0,
                RealNotLiquidValue = 0,
            }));

            return list;

        }

        public static ForecastList GroupMonthly(ForecastList list, DateTime startDate, DateTime lastDate)
        {
            var listNewItems = list.Items.OrderBy(a => a.DateReference).GroupBy(a => new { a.DateReference.Year, a.DateReference.Month }, (key, group) =>
                  new ForecastItem
                  {
                      DateReference = new DateTime(key.Year, key.Month, 1).AddMonths(1).AddDays(-1),
                      NominalLiquidValue = group.Sum(a => a.NominalLiquidValue),
                      NominalNotLiquidValue = group.Sum(a => a.NominalNotLiquidValue),
                      NominalCumulatedAmount = group.Sum(a => a.NominalCumulatedAmount),
                      RealCumulatedAmount = group.Sum(a => a.RealCumulatedAmount),
                      RealLiquidValue = group.Sum(a => a.RealLiquidValue),
                      RealNotLiquidValue = group.Sum(a => a.RealNotLiquidValue)
                  }
              ).ToList();

            int months = ((lastDate.Year - startDate.Year) * 12) + lastDate.Month - startDate.Month;

            if(listNewItems.Count < months)
            {
                for(var date = startDate; date < lastDate; date = date.AddMonths(1))
                {
                    
                    listNewItems.Add(new ForecastItem
                    {
                        DateReference = new DateTime(date.Year, date.Month, 1).GetLastDayOfThisMonth(),
                        NominalLiquidValue = 0,
                        NominalNotLiquidValue = 0,
                        NominalCumulatedAmount = 0,
                        RealCumulatedAmount = 0,
                        RealLiquidValue = 0,
                        RealNotLiquidValue = 0
                    });                                      
                }
            }            
            
            return new ForecastList()
            {
                Items = listNewItems,
                Type = list.Type
            };
        }

        private static List<DateTime> GetAllDatesBetween(DateTime minDate, DateTime maxDate)
        {
            var dates = new List<DateTime>();

            for(var date = minDate; date < maxDate; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            return dates;
        }

        private static DayMovimentation CheckIfMustUpdateBalance(ForecastList spendingsDaily, ForecastList incomesDaily, ForecastList loanDaily, ForecastList fgtsDaily, DateTime date)
        {
            DayMovimentation dayMovimentation = new();
            if (incomesDaily.Items.Any(a => a.DateReference == date))            
                dayMovimentation.IncomesReceived = incomesDaily.Items.Where(a => a.DateReference == date).Sum(a => a.NominalLiquidValue);
            
            if (spendingsDaily.Items.Any(a => a.DateReference == date))            
                dayMovimentation.Spendings = spendingsDaily.Items.Where(a => a.DateReference == date).Sum(a => a.NominalLiquidValue);
            
            if (loanDaily.Items.Any(a => a.DateReference == date))            
                dayMovimentation.LoansPayment = loanDaily.Items.Where(a => a.DateReference == date).Sum(a => a.NominalLiquidValue);
            
            if(fgtsDaily.Items.Any(a => a.DateReference == date && a.NominalLiquidValue > 0))            
                dayMovimentation.FGTSWithdraw = fgtsDaily.Items.Where(a => a.DateReference == date).Sum(a => a.NominalLiquidValue);
            

            return dayMovimentation;
        }
    }
}
