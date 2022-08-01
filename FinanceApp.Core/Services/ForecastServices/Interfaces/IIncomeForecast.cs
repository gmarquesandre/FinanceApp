using FinanceApp.Shared;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices.Interfaces
{
    public interface IIncomeForecast 
    {
        EItemType Item { get; }
        ForecastList GetForecast(List<IncomeDto> incomeDtos, EForecastType forecastType, DateTime maxDate, DateTime? minDate = null);
        List<IncomeSpread> GetIncomesSpreadList(List<IncomeDto> incomesDto, DateTime maxYearMonth, DateTime? minDateInput = null);
    }
}