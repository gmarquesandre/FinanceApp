using FinanceApp.Api.Controllers.CrudController.Base;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces;
using FinanceApp.Shared.Dto.PrivateFixedInvestment;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers.CrudController
{
    [Route("[controller]")]
    [ApiController]
    public class PrivateFixedIncomeController : CrudControllerBase<IPrivateFixedIncomeService, PrivateFixedIncomeDto, CreatePrivateFixedIncome, UpdatePrivateFixedIncome>
    {

        public PrivateFixedIncomeController(IPrivateFixedIncomeService service) : base(service)
        {
        }

    }
}