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

            bool updateValueWithCdiIndex = balance.UpdateValueWithCdiIndex;

            double percentageCdi = balance.PercentageCdi ?? 1.00;

            if (balance != null)
            {
                balanceTitlesList.Add(new BalanceTitle()
                {
                    DateReference = balance.UpdateDateTime ?? balance.CreationTime,
                    Value = balance.Value,
                    PercentageCdi = balance.PercentageCdi,
                    UpdateValueWithCdiIndex = balance.UpdateValueWithCdiIndex
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
                                balanceTitlesList.Add(new BalanceTitle()
                                {
                                    DateReference = date,
                                    Value = incomesDay,
                                    UpdateValueWithCdiIndex = updateBalance,
                                    PercentageCdi = percentageCdi
                                });
                            }

                        }
                        else if (incomesDay < loansDay + spendingsDay)
                        {
                            double leftValue = spendingsDay + loansDay - incomesDay;

                            double totalSpendingDayValue = Convert.ToDouble(spendingsDay + loansDay);

                            balanceTitlesList.ForEach(async title =>
                                {

                                    var titleUpdated = await GetCurrentValueOfTitle(title.DateReference, title.Value, date, title!.UpdateValueWithCdiIndex, title.PercentageCdi ?? 1.00);


                                    if (titleUpdated.liquidValue > totalSpendingDayValue)
                                    {
                                        //Atualiza titulo 

                                        //Acho que aqui precisa deflacionar o valor liquido novo e subtrair - Pegar os prints do btg
                                        var newLiquidValue = titleUpdated.liquidValue - totalSpendingDayValue;

                                        if (title!.UpdateValueWithCdiIndex)
                                        {

                                        }
                                        else
                                        {
                                            title.Value = newLiquidValue;
                                        }


                                        //Zera
                                        totalSpendingDayValue = 0;
                                        //Finaliza loop



                                        return;
                                    }
                                    else if (titleUpdated.liquidValue < totalSpendingDayValue)
                                    {
                                        totalSpendingDayValue -= titleUpdated.liquidValue;
                                        balanceTitlesList.Remove(title);
                                    }
                                    else
                                    {
                                        totalSpendingDayValue = 0;
                                        balanceTitlesList.Remove(title);
                                    }


                                }
                            );


                            if (totalSpendingDayValue > 0)
                            {

                            }



                        }
                        //double balanceFinal = balanceInitial + incomesDay - spendingsDay - loansDay;

                        //balanceInitial += balanceInitial;
                    }


                    date = date.AddDays(1);
                }



            }



            return null;
        }

        //Função apenas para saldo em conta corrente que é atualizado pelo CDI
        //sempre o IR será considerado como 22,5% e o IOF sempre será utilizado a partir do ultimo dia atualizado
        private async Task<(double grossValue, double liquidValue, double iof, double incomeTax)> GetCurrentValueOfTitle(DateTime dateInvestment, double investmentValue, DateTime date, bool updateWithCdiIndex, double indexPercentage)
        {
            if (investmentValue > 0 && updateWithCdiIndex)
            {
                var apprecitation = await _indexService.GetIndexValueBetweenDates(EIndex.CDI,
                                            dateInvestment,
                                            date,
                                            indexPercentage);

                var grossValue = Convert.ToDouble(investmentValue) * apprecitation;

                var incomeTaxPercentage = 0.225;

                var iofPercentage = _indexService.GetIof((date - dateInvestment).Days);

                var iofValue = iofPercentage * (grossValue - Convert.ToDouble(investmentValue));

                var grossValueAfterIof = grossValue - iofValue;

                var incomeTaxValue = incomeTaxPercentage * (grossValueAfterIof - Convert.ToDouble(investmentValue));

                var liquidValue = grossValueAfterIof - incomeTaxValue;

                return (grossValue, liquidValue, iofValue, incomeTaxValue);
            }
            else if(investmentValue < 0)
            {

                //Aqui poderia colocar o juros de empréstimo da conta
                return (0, 0, 0, 0);
            }
            else
            {

                return (Convert.ToDouble(investmentValue), Convert.ToDouble(investmentValue), 0,0);
            }

        }
    }
    public class BalanceTitle
    {
        public DateTime DateReference { get; set; }
        public double Value { get; set; }
        public bool UpdateValueWithCdiIndex { get; set; }
        public double? PercentageCdi { get; set; }
    }
}
