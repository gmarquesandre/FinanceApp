using FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces;
using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Interfaces;
using FinanceApp.Core.Services.ForecastServices.Interfaces;
using FinanceApp.FinanceData.Services;
using FinanceApp.Shared;
using FinanceApp.Shared.Dto;
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

        public async Task<List<ForecastList>> GetForecast(DateTime currentDate)
        {
            var forecastTotalList = new List<ForecastItem>();


            var forecastParameters = await _forecastParametersService.GetAsync();

            DateTime maxYearMonth = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1).AddMonths(13).AddDays(-1);

            var spendingsDaily = await _spendingService.GetForecast(EForecastType.Daily, maxYearMonth, currentDate);
            var incomesDaily = await _incomeService.GetForecast(EForecastType.Daily, maxYearMonth, currentDate);
            var loanDaily = await _loanService.GetForecast(EForecastType.Daily, maxYearMonth, currentDate);
            var fgtsDaily = await _fgtsService.GetForecast(EForecastType.Daily, maxYearMonth, currentDate);

            var balance = await _currentBalanceService.GetAsync();

            var balanceTitlesList = new List<DefaultTitleInput>();

            bool updateValueWithCdiIndex = balance.UpdateValueWithCdiIndex;

            double percentageCdi = balance.PercentageCdi ?? 1.00;

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


            for (DateTime date = DateTime.Now.Date.AddDays(1); date <= maxYearMonth; date = date.AddDays(1))
            {              
                //variavel responsável por avisar se há alterações no balanço
                (DayMovimentation dayMovimentation, bool updateBalance ) = CheckIfMustUpdateBalance(spendingsDaily, incomesDaily, loanDaily, fgtsDaily, date);

                if (updateBalance)
                {
                    if (balanceTitlesList.Any(a => a.InvestmentValue < 0.00))
                    {

                        dayMovimentation.OwingValue = -balanceTitlesList.Where(a => a.InvestmentValue < 0.00).Select(async a =>
                        {
                            a.Date = date;
                            return await _titleService.GetCurrentValueOfTitle(a);
                        }).Select(a => a.Result).Sum(a => a.LiquidValue);                        

                    }

                    
                    if (dayMovimentation.PositiveBalanceDay)
                    {
                        balanceTitlesList = balanceTitlesList.Where(a => a.InvestmentValue > 0.00).ToList();

                        double newInvestmentValue = dayMovimentation.ResultValue;

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
                    
                    else if (dayMovimentation.NegativeBalanceDay)
                    {
                        double leftValue = -dayMovimentation.ResultValue;                        

                        foreach(var title in balanceTitlesList.Where(a => a.InvestmentValue > 0.00).ToList())
                        {
                            if (Math.Round(leftValue,2) <= 0.00)
                                break;

                            title.Date = date;
                            var (titleOutput, withdraw) = await _titleService.GetCurrentTitleAfterWithdraw(title, leftValue);

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
                        balanceTitlesList = balanceTitlesList.Where(a => Math.Round(a.InvestmentValue,2) != 0.00).ToList();


                        if (leftValue > 0)
                        {
                            //Remove titulo de divida atual e atualiza
                            balanceTitlesList = balanceTitlesList.Where(a => Math.Round(a.InvestmentValue,2) > 0.00).ToList();
                            balanceTitlesList.Add(new DefaultTitleInput()
                            {
                                DateInvestment = date,
                                InvestmentValue = -leftValue,
                                IndexPercentage = forecastParameters.PercentageCdiLoan,
                                Index = EIndex.CDI,
                                AdditionalFixedInterest = 0.00,
                                TypePrivateFixedIncome = ETypePrivateFixedIncome.BalanceCDB
                            });
                        }
                    }
                    else throw new Exception("Erro ao realizar cálculos");

                }

                if (date.AddDays(1).Day == 1)
                {

                    var valorTitulos = balanceTitlesList.Select(async a =>
                    {
                        a.Date = date;
                        return await _titleService.GetCurrentValueOfTitle(a);
                    }).Select(a => a.Result).Sum(a => a.LiquidValue);

                    double valorTitulosReal = await _indexService.GetRealValue(date, valorTitulos);

                    double notLiquidNominal = fgtsDaily.Items.Where(a => a.DateReference == date).Sum(a => a.NominalCumulatedAmount);

                    double notLiquidReal = await _indexService.GetRealValue(date, notLiquidNominal);


                    forecastTotalList.Add(new ForecastItem()
                    {
                        RealLiquidValue = valorTitulosReal,
                        NominalLiquidValue = valorTitulos,
                        NominalCumulatedAmount = valorTitulos,
                        NominalNotLiquidValue = notLiquidNominal,
                        RealCumulatedAmount = notLiquidReal,
                        DateReference = date,
                    });
                }
            }


            var totalDaily = new ForecastList()
            {
                Items = forecastTotalList,
                Type = EItemType.Total
            };

            var output = new List<ForecastList>()
            {
                totalDaily,
                incomesDaily,
                spendingsDaily,
                loanDaily,
                fgtsDaily
            };

            return output;
      
        }

        private (DayMovimentation,bool) CheckIfMustUpdateBalance(ForecastList spendingsDaily, ForecastList incomesDaily, ForecastList loanDaily, ForecastList fgtsDaily, DateTime date)
        {

            bool updateBalance = false;
            DayMovimentation dayMovimentation = new();
            if (incomesDaily.Items.Any(a => a.DateReference == date))
            {
                dayMovimentation.IncomesReceived = incomesDaily.Items.Where(a => a.DateReference == date).Sum(a => a.NominalLiquidValue);
                updateBalance = true;
            }
            if (spendingsDaily.Items.Any(a => a.DateReference == date))
            {
                dayMovimentation.Spendings = spendingsDaily.Items.Where(a => a.DateReference == date).Sum(a => a.NominalLiquidValue);
                updateBalance = true;
            }
            if (loanDaily.Items.Any(a => a.DateReference == date))
            {
                dayMovimentation.LoansPayment = loanDaily.Items.Where(a => a.DateReference == date).Sum(a => a.NominalLiquidValue);
                updateBalance = true;
            }
            if(fgtsDaily.Items.Any(a => a.DateReference == date && a.NominalLiquidValue > 0))
            {
                dayMovimentation.FGTSWithdraw = fgtsDaily.Items.Where(a => a.DateReference == date).Sum(a => a.NominalLiquidValue);
                updateBalance = true;
            }

            return (dayMovimentation, updateBalance);
        }
    }
}
