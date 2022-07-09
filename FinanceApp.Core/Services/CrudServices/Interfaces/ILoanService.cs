using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Loan;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface ILoanService : ITransientService
    {
        Task<LoanDto> AddAsync(CreateLoan input);
        Task<Result> DeleteAsync(int id);
        Task<List<LoanDto>> GetAsync();
        Task<LoanDto> GetAsync(int id);
        Task<Result> UpdateAsync(UpdateLoan input);
        Task<ForecastList> GetForecast(EForecastType forecastType, DateTime maxYearMonth);
    }
}