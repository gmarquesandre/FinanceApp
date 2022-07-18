using FinanceApp.Shared.Dto.ForecastParameters;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface IForecastParametersService : ITransientService
    {
        Task<ForecastParametersDto> AddOrUpdateAsync(CreateOrUpdateForecastParameters input);
        Task<Result> DeleteAsync();
        Task<ForecastParametersDto> GetAsync();
    }
}