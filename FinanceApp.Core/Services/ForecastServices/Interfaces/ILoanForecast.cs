using FinanceApp.Shared;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Loan;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices.Interfaces
{
    public interface ILoanForecast : IScopedService
    {
        ForecastList GetForecast(List<LoanDto> loanDtos, EForecastType forecastType, DateTime maxDate, DateTime minDate);
        List<LoanSpread> GetLoansSpreadList(List<LoanDto> loanDto, DateTime maxYearMonth, DateTime minDateInput);
    }
}