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

        public static EItemType Item => EItemType.Loan;

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

            decimal amortization = loan.LoanValue / (decimal)loan.MonthsPayment;

            double monthsInAYear = 12.00;
            double interestRateMonthMultipliler = Math.Pow(1.00 + (Convert.ToDouble(loan.InterestRate) / 100.00), 1.00 / monthsInAYear) - 1;

            maxDate = maxDate < loan.InitialDate.AddMonths(loan.MonthsPayment) ? maxDate : loan.InitialDate.AddMonths(loan.MonthsPayment);

            for (DateTime date = minDate; date <= maxDate; date = date.AddMonths(1))
            {
                var loanSpread = _mapper.Map<LoanSpread>(loan);

                int monthsPaid =  MonthDiff(loan.InitialDate, date);

                decimal interestValueParcel = (decimal)interestRateMonthMultipliler * loan.LoanValue * (1 -( (decimal)monthsPaid / (decimal)loan.MonthsPayment));

                loanSpread.LoanAmortizationValue = amortization;
                loanSpread.Date = date;
                loanSpread.LoanInterestValue = interestValueParcel;
                loanSpread.LoanValueMonth = amortization + interestValueParcel;

                loanSpreadList.Add(loanSpread);

            }

            return loanSpreadList;

        }

        private List<LoanSpread> GetPriceValues(LoanDto loan, DateTime minDate, DateTime maxDate)
        {
            var loanSpreadList = new List<LoanSpread>();

            double monthsInAYear= 12.00;
            double interestRateMonthMultipliler = Math.Pow(1.00 + (Convert.ToDouble(loan.InterestRate) / 100.00), 1.00 / monthsInAYear) - 1;
            
            double InterestValue = Math.Pow(1+interestRateMonthMultipliler, loan.MonthsPayment);

            decimal valueParcel = loan.LoanValue * (decimal)(InterestValue * interestRateMonthMultipliler / (InterestValue - 1.00) );

            maxDate = maxDate < loan.InitialDate ? maxDate : loan.InitialDate;
            for (DateTime date = minDate; date <= maxDate; date = date.AddMonths(1))
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

        public static int MonthDiff(DateTime d1, DateTime d2)
        {
            int m1;
            int m2;
            if (d1 < d2)
            {
                m1 = (d2.Month - d1.Month);//for years
                m2 = (d2.Year - d1.Year) * 12; //for months
            }
            else
            {
                m1 = (d1.Month - d2.Month);//for years
                m2 = (d1.Year - d2.Year) * 12; //for months
            }

            return m1 + m2;
        }
    }
}
