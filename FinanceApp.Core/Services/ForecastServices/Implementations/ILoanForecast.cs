using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Loan;

namespace FinanceApp.Core.Services.ForecastServices.Implementations
{
    public interface ILoanForecast
    {
        List<LoanSpread> GetLoanSpreadList(List<LoanDto> loanDto, DateTime maxYearMonth, DateTime? minDateInput = null);
        List<ForecastItem> GetMonthlyForecast(List<LoanDto> incomes, DateTime maxDate, DateTime? minDate = null);
    }
}