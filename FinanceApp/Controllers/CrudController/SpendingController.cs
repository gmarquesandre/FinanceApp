using FinanceApp.Api.Controllers.CrudController.Base;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces;
using FinanceApp.Shared.Dto.Spending;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers.CrudController
{
    [Route("[controller]")]
    [ApiController]
    public class SpendingController : CrudControllerBase<ISpendingService, SpendingDto, CreateSpending, UpdateSpending>
    {

        public SpendingController(ISpendingService service) : base(service)
        {
        }

    }
}