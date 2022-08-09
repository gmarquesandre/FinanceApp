using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.FGTS;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices.Interfaces
{
    public interface IFGTSForecast
    {
        EItemType Item { get; }
        ForecastList GetForecastAsync(FGTSDto fgtsDto, EForecastType forecastType, DateTime maxDate, DateTime? minDate = null);
        List<FGTSSpread> GetFGTSsSpreadListAsync(FGTSDto fgtsDto, DateTime maxYearMonth, DateTime? minDateInput = null);
    }
}