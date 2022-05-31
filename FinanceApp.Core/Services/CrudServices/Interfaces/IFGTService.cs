using FinanceApp.Shared.Dto.FGTS;
using FinanceApp.Shared.Models.CommonTables;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface IFGTSService
    {
        Task<FGTSDto> AddOrUpdateAsync(CreateOrUpdateFGTS input, CustomIdentityUser user);
        Task<Result> DeleteAsync(CustomIdentityUser user);
        Task<FGTSDto> GetAsync(CustomIdentityUser user);
    }
}