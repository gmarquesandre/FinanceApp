using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface IIncomeService : ITransientService
    {
        Task<IncomeDto> AddAsync(CreateIncome input);
        Task<Result> DeleteAsync(int id);
        Task<List<IncomeDto>> GetAsync();
        Task<IncomeDto> GetAsync(int id);
        Task<Result> UpdateAsync(UpdateIncome input);
        Task<ForecastList> GetForecast(EForecastType forecastType, DateTime maxYearMonth);
    }
}