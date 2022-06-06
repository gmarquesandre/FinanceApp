using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices.Implementations
{
    public interface IIncomeForecast : ITransientService
    {
        EItemType Item { get; }
        ForecastList GetForecast(List<IncomeDto> incomeDtos, EForecastType forecastType, DateTime maxDate, DateTime? minDate = null);
        List<IncomeSpread> GetIncomesSpreadList(List<IncomeDto> incomesDto, DateTime maxYearMonth, DateTime? minDateInput = null);
    }
}