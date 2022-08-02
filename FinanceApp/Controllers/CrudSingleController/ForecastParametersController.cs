using FinanceApp.Api.Controllers.CrudController.Base;
using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Interfaces;
using FinanceApp.Shared.Dto.ForecastParameters;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers.CrudSingleController
{
    [Route("[controller]")]
    [ApiController]
    public class ForecastParametersController : CrudSingleControllerBase<IForecastParametersService, ForecastParametersDto, CreateOrUpdateForecastParameters>
    {

        public ForecastParametersController(IForecastParametersService service) : base(service)
        {
        }
    }
}