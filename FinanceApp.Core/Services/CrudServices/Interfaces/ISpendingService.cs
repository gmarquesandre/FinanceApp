using FinanceApp.Shared;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Spending;
using FinanceApp.Shared.Enum;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface ISpendingService : IScopedService
    {
        Task<SpendingDto> AddAsync(CreateSpending input);
        Task<Result> DeleteAsync(int id);
        Task<List<SpendingDto>> GetAsync();
        Task<SpendingDto> GetAsync(int id);
        Task<Result> UpdateAsync(UpdateSpending input);
        Task<ForecastList> GetForecast(EForecastType forecastType, DateTime maxYearMonth, DateTime currentDate);
    }
}