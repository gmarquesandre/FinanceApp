using FinanceApp.Core.Services.CrudServices.CrudDefault.Base;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces;
using FinanceApp.Shared.Dto.Income;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces
{
    public interface IIncomeService : ICommand<IncomeDto, CreateIncome, UpdateIncome>,
        IQuery<IncomeDto>, IForecast<IncomeDto>
    {
    }
}