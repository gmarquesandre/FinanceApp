using FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces;
using FinanceApp.Shared.Dto.Spending;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces
{
    public interface ISpendingService : IForecast<SpendingDto>
    {
    }
}