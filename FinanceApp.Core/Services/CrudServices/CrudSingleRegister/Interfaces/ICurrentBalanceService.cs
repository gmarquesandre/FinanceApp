using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Base;
using FinanceApp.Shared.Dto.CurrentBalance;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Interfaces
{
    public interface ICurrentBalanceService : ICrudSingleBase<CurrentBalanceDto, CreateOrUpdateCurrentBalance>
    {
    }
}