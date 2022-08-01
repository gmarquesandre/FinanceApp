using FinanceApp.Shared;
using FinanceApp.Shared.Dto.FGTS;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface IFGTSService 
    {
        Task<FGTSDto> AddOrUpdateAsync(CreateOrUpdateFGTS input);
        Task<Result> DeleteAsync();
        Task<FGTSDto> GetAsync();
    }
}