using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Base;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces
{
    public interface IForecast<TDto>
        where TDto : StandardDto

    {
        Task<ForecastList> GetForecast(EForecastType type, DateTime maxYearMonth, DateTime currentDate);
    }
}
