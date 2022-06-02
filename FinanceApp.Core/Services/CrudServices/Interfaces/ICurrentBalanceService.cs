using FinanceApp.Shared.Dto.CurrentBalance;
using FinanceApp.Shared.Models.CommonTables;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface ICurrentBalanceService
    {
        Task<CurrentBalanceDto> AddOrUpdateAsync(CreateOrUpdateCurrentBalance input, CustomIdentityUser user);
        Task<Result> DeleteAsync(CustomIdentityUser user);
        Task<CurrentBalanceDto> GetAsync(CustomIdentityUser user);
    }
}