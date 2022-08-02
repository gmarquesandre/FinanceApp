using FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces;
using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Base;
using FinanceApp.Shared.Dto.FGTS;
using FinanceApp.Shared.Dto.ForecastParameters;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Interfaces
{
    public interface IForecastParametersService : ICommandSingle<ForecastParametersDto, CreateOrUpdateForecastParameters>, IQuerySingle<ForecastParametersDto>
    {
    }
}