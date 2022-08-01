using FinanceApp.Core.Services.CrudServices.Base;
using FinanceApp.Shared.Dto.CurrentBalance;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister
{
    public interface ICurrentBalanceService : ICommandSingle<CurrentBalanceDto, CreateOrUpdateCurrentBalance>
    {
        Task<CurrentBalanceDto> GetAsync();
    }
}