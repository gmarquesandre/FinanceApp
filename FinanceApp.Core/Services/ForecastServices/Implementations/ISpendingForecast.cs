using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Spending;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices.Implementations
{
    public interface ISpendingForecast : ITransientService
    {
        EItemType Item { get; }

        ForecastList GetDailyForecast(List<SpendingDto> spendingDtos, DateTime maxDate, DateTime? minDate = null);
        ForecastList GetMonthlyForecast(List<SpendingDto> spendingDtos, DateTime maxDate, DateTime? minDate = null);
        List<SpendingSpread> GetSpendingsSpreadList(List<SpendingDto> spendingsDto, DateTime maxYearMonth, DateTime? minDateInput = null);
    }
}