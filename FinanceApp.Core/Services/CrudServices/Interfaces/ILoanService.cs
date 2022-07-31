using FinanceApp.Shared;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Loan;
using FinanceApp.Shared.Enum;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface ILoanService : IScopedService
    {
        Task<LoanDto> AddAsync(CreateLoan input);
        Task<Result> DeleteAsync(int id);
        Task<List<LoanDto>> GetAsync();
        Task<LoanDto> GetAsync(int id);
        Task<Result> UpdateAsync(UpdateLoan input);
        Task<ForecastList> GetForecast(EForecastType forecastType, DateTime maxYearMonth, DateTime currentDate);
    }
}