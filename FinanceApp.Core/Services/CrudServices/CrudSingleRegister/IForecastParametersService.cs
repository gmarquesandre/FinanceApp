using FinanceApp.Shared;
using FinanceApp.Shared.Dto.ForecastParameters;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister
{
    public interface IForecastParametersService
    {
        Task<ForecastParametersDto> AddOrUpdateAsync(CreateOrUpdateForecastParameters input);
        Task<Result> DeleteAsync();
        Task<ForecastParametersDto> GetAsync();
    }
}