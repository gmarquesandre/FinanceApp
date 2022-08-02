using FinanceApp.Api.Controllers.CrudController.Base;
using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Interfaces;
using FinanceApp.Shared.Dto.FGTS;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers.CrudSingleController
{
    [Route("[controller]")]
    [ApiController]
    public class FGTSController : CrudSingleControllerBase<IFGTSService, FGTSDto, CreateOrUpdateFGTS>
    {

        public FGTSController(IFGTSService service) : base(service)
        {
        }
    }
}