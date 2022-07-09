using FinanceApp.Shared.Dto.CurrentBalance;
using FinanceApp.Shared.Models.CommonTables;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface ICurrentBalanceService : ITransientService
    {
        Task<CurrentBalanceDto> AddOrUpdateAsync(CreateOrUpdateCurrentBalance input);
        Task<Result> DeleteAsync();
        Task<CurrentBalanceDto> GetAsync();
    }
}