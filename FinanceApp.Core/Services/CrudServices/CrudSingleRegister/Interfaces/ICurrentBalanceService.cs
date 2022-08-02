using FinanceApp.Core.Services.CrudServices.CrudDefault.Base;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces;
using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Base;
using FinanceApp.Shared.Dto.CurrentBalance;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Interfaces
{
    public interface ICurrentBalanceService : ICommandSingle<CurrentBalanceDto, CreateOrUpdateCurrentBalance>, IQuerySingle<CurrentBalanceDto>
    {
    }
}