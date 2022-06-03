using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Implementations;
using FinanceApp.Shared.Dto.Loan;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices.Implementations
{
    public class LoanForecast
    {
        private readonly IMapper _mapper;
        
        public LoanForecast(IMapper mapper)
        {
            _mapper = mapper;
        }

        public EItemType Item => EItemType.Loan;

        public List<LoanSpread> GetLoanSpreadList(List<LoanDto> loanDto, DateTime maxYearMonth, DateTime? minDateInput = null)
        {

            maxYearMonth = new DateTime(maxYearMonth.Year, maxYearMonth.Month, 1).AddMonths(1).AddDays(-1);
            DateTime minDate = minDateInput ?? DateTime.Now.Date;

            var loanSpreadList = new List<LoanSpread>();


            foreach(var loan in loanDto)
            {
                
                if(loan.Type == EPaymentType.PRICE)
                {
                    loanSpreadList.AddRange(GetPriceValues(loan, minDate, maxYearMonth));
                }
                else if (loan.Type == EPaymentType.SAC)
                {
                    //Valor variável
                    loanSpreadList.AddRange(GetSacValues(loan, minDate, maxYearMonth));
                }
            }

            return loanSpreadList;

        }

        private List<LoanSpread> GetSacValues(LoanDto loan, DateTime minDate, DateTime maxDate)
        {
            var loanSpreadList = new List<LoanSpread>();

            decimal amortization = loan.LoanValue / loan.MonthsPayment;
            
            double interestRateMonthMultipliler = Math.Pow(1.00 + (Convert.ToDouble(loan.InterestRate) / 100.00), 1 / 12);

            for (DateTime date = minDate; date <= maxDate; date.AddMonths(1))
            {
                var loanSpread = _mapper.Map<LoanSpread>(loan);

                int monthsPaid = date.Year * date.Month - loan.InitialDate.Year * loan.InitialDate.Month;

                decimal interestValueParcel = (decimal)interestRateMonthMultipliler * loan.LoanValue * (1 - Convert.ToDecimal(monthsPaid / loan.MonthsPayment));

                loanSpread.LoanAmortizationValue = amortization;
                loanSpread.Date = date;
                loanSpread.LoanValueMonth = amortization + interestValueParcel;

                loanSpreadList.Add(loanSpread);

            }

            return loanSpreadList;

        }

        private List<LoanSpread> GetPriceValues(LoanDto loan, DateTime minDate, DateTime maxDate)
        {
            var loanSpreadList = new List<LoanSpread>();
            
            double interestRateMonthMultipliler = Math.Pow(1.00 + (Convert.ToDouble(loan.InterestRate) / 100.00), 1 / 12);
            
            double InterestValue = Math.Pow(1 + interestRateMonthMultipliler, loan.MonthsPayment);

            decimal valueParcel = loan.LoanValue * (decimal)(InterestValue * interestRateMonthMultipliler / (interestRateMonthMultipliler - 1) );            
            
            for(DateTime date = minDate; date <= maxDate; date.AddMonths(1))
            {
                var loanSpread = _mapper.Map<LoanSpread>(loan);

                loanSpread.Date = date;

                //Algum dia revisitar isto aqui pra calcular certo a amortização
                loanSpread.LoanAmortizationValue = valueParcel;
                loanSpread.LoanValueMonth = valueParcel;

                loanSpreadList.Add(loanSpread);

            }
            return loanSpreadList;
        }
    }
}
