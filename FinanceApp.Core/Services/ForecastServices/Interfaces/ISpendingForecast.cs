using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Spending;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices.Interfaces
{
    public interface ISpendingForecast : ITransientService
    {
        EItemType Item { get; }
        ForecastList GetForecast(List<SpendingDto> spendingDtos, EForecastType forecastType, DateTime maxDate, DateTime? minDate = null);
        List<SpendingSpread> GetSpendingsSpreadList(List<SpendingDto> spendingsDto, DateTime maxYearMonth, DateTime? minDateInput = null);
    }
}