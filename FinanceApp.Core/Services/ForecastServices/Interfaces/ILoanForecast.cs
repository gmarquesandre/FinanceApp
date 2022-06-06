using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Loan;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices.Implementations
{
    public interface ILoanForecast : ITransientService
    {
        ForecastList GetForecast(List<LoanDto> loanDtos, EForecastType forecastType, DateTime maxDate, DateTime? minDate = null);
        List<LoanSpread> GetLoansSpreadList(List<LoanDto> loanDto, DateTime maxYearMonth, DateTime? minDateInput = null);
    }
}