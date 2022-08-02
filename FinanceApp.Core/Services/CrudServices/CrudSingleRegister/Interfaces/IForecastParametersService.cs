using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Base;
using FinanceApp.Shared.Dto.ForecastParameters;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Interfaces
{
    public interface IForecastParametersService : ICrudSingleBase<ForecastParametersDto, CreateOrUpdateForecastParameters>
    {
    }
}