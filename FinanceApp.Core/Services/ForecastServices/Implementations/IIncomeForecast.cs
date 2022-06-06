using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices.Implementations
{
    public interface IIncomeForecast
    {
        EItemType Item { get; }

        List<ForecastItem> GetDailyForecast(List<IncomeDto> incomesDto, DateTime maxDate, DateTime? minDate = null);
        List<IncomeSpread> GetIncomesSpreadList(List<IncomeDto> incomesDto, DateTime maxYearMonth, DateTime? minDateInput = null);
        List<ForecastItem> GetMonthlyForecast(List<IncomeDto> incomes, DateTime maxDate, DateTime? minDate = null);
    }
}