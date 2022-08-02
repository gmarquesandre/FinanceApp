using FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces;
using FinanceApp.Shared.Dto.PrivateFixedInvestment;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces
{
    public interface IPrivateFixedIncomeService : ICommand<PrivateFixedIncomeDto, CreatePrivateFixedIncome, UpdatePrivateFixedIncome>,
        IQuery<PrivateFixedIncomeDto>
    {
    }
}