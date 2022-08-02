using FinanceApp.Core.Services.CrudServices.CrudDefault.Base;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces;
using FinanceApp.Shared.Dto.Income;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces
{
    public interface IIncomeService : ICrudBase<IncomeDto, CreateIncome, UpdateIncome>, IForecast<IncomeDto>
    {
    }
}