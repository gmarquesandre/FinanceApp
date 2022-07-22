using FinanceApp.Shared.Dto.FGTS;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface IFGTSService : IScopedService
    {
        Task<FGTSDto> AddOrUpdateAsync(CreateOrUpdateFGTS input);
        Task<Result> DeleteAsync();
        Task<FGTSDto> GetAsync();
    }
}