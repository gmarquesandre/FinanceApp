using FinanceApp.Shared.Dto.FGTS;
using FinanceApp.Shared.Models.CommonTables;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface IFGTSService : ITransientService
    {
        Task<FGTSDto> AddOrUpdateAsync(CreateOrUpdateFGTS input);
        Task<Result> DeleteAsync();
        Task<FGTSDto> GetAsync();
    }
}