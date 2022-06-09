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

            decimal balanceInitial = 0.00M;

            if (balance != null)
            {
                balanceInitial = balance.CurrentBalance;
            }

            //Criar lista de "titulos" para todas entradas e saídas com taxas atualização de referencia
            //Adicionar titulos liquidos quando tiver renda fixa e tesouro direto           
            //Estes titulos na conta corrente, caso sejam atualizados por algum indice, devem sempre ter IOF e IR fixo de 27,5%,
            //afim de facilitar calculos e garantir não exibir um valor maior do que deveria,
            //dado que o tempo para vencimento do titulo nem sempre está claro 
            //Campos:
            //Data Investimento
            //Valor Investido
            //Valor Liquido
            //
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

                        decimal balanceFinal = balanceInitial + incomesDay - spendingsDay - loansDay;

                        balanceInitial += balanceInitial;
                    }


                    date = date.AddDays(1);
                }



            }



            return null;
        }

    }
}
