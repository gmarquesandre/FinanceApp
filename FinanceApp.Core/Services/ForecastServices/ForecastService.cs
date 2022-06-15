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
        public IIndexService _indexService;

        public ForecastService(ISpendingService spendingService, 
            IIncomeService incomeService, 
            ICurrentBalanceService currentBalanceService, 
            ILoanService loanService, 
            IIndexService indexService)
        {
            _spendingService = spendingService;
            _incomeService = incomeService;
            _currentBalanceService = currentBalanceService;
            _loanService = loanService;
            _indexService = indexService;
        }

        public async Task<List<ForecastList>> GetForecast(CustomIdentityUser user)
        {
            DateTime maxYearMonth = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1).AddMonths(13).AddDays(-1);

            var spendingsDaily = await _spendingService.GetForecast(user, EForecastType.Daily, maxYearMonth);
            var incomesDaily = await _incomeService.GetForecast(user, EForecastType.Daily, maxYearMonth);
            var loanDaily = await _loanService.GetForecast(user, EForecastType.Daily, maxYearMonth);

            var balance = await _currentBalanceService.GetAsync(user);

                var balanceTitlesList = new List<BalanceTitle>();
            
            if (balance != null)
            {
                balanceTitlesList.Add(new BalanceTitle()
                {
                    DateReference = balance.UpdateDateTime ?? balance.CreationTime,
                    Value = balance.CurrentBalance
                });                
            }                       
            
            
            for (DateTime date = DateTime.Now.Date; date <= maxYearMonth; date = date.AddMonths(1))
            {

                while (date.Day > 1)
                {

                    decimal loansDay = 0.00M;
                    decimal incomesDay = 0.00M;
                    decimal spendingsDay = 0.00M;

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
                        if(incomesDay > loansDay + spendingsDay)
                        {
                            balanceTitlesList.Add(new BalanceTitle()
                            {
                                DateReference = date,
                                Value = incomesDay,
                            });
                        }
                        else if( incomesDay < loansDay + spendingsDay)
                        {
                            decimal leftValue = spendingsDay + loansDay - incomesDay;

                            double totalSpendingDayValue = Convert.ToDouble(spendingsDay + loansDay);

                            balanceTitlesList.ForEach(async title =>
                                {

                                    var titleLiquidValue = await UpdateBalanceTitleValue(title.DateReference, title.Value, date);
                                    

                                    if(titleLiquidValue > totalSpendingDayValue)
                                    {
                                        //Atualiza titulo 

                                        //Acho que aqui precisa deflacionar o valor liquido novo e subtrair - Pegar os prints do btg
                                        var newLiquidValue = titleLiquidValue - totalSpendingDayValue;

                                        //title.Value = titleVal
                                        

                                        //Zera
                                        totalSpendingDayValue = 0;
                                        //Finaliza loop
                                        return;
                                    }
                                    else if(titleLiquidValue < totalSpendingDayValue)
                                    {
                                        totalSpendingDayValue -= titleLiquidValue;
                                    }
                                    else
                                    {
                                        titleLiquidValue = 0;
                                        balanceTitlesList.Remove(title);
                                    }


                                }
                            );


                            if(totalSpendingDayValue > 0)
                            {

                            }



                        }
                        //decimal balanceFinal = balanceInitial + incomesDay - spendingsDay - loansDay;

                        //balanceInitial += balanceInitial;
                    }


                    date = date.AddDays(1);
                }



            }



            return null;
        }

        private async Task<double> UpdateBalanceTitleValue(DateTime dateInvestment, decimal investmentValue, DateTime date)
        {
            if(investmentValue > 0) { 
                var apprecitation = await _indexService.GetIndexValueBetweenDates(EIndex.CDI,
                                            dateInvestment,
                                            date);

                var incomeTax = 0.275;

                var iof = _indexService.GetIof((date - dateInvestment).Days);

                var finalValue = Convert.ToDouble(investmentValue) + (Convert.ToDouble(investmentValue) - incomeTax) * ( 1 - iof) * ( 1 - incomeTax) ;

                return finalValue;
            }

            //Aqui poderia colocar o juros de empréstimo da conta
            return -1;

        }
    }
    public class BalanceTitle
    {
        public DateTime DateReference { get; set; }
        public decimal Value { get; set; }        
    }
}
