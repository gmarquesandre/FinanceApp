using FinanceApp.Core.Services.CrudServices.CrudDefault.Base;
using FinanceApp.Shared.Dto.CurrentBalance;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister
{
    public interface ICurrentBalanceService : ICommandSingle<CurrentBalanceDto, CreateOrUpdateCurrentBalance>
    {
        Task<CurrentBalanceDto> GetAsync();
    }
}