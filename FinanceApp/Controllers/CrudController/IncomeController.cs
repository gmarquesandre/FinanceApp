using FinanceApp.Api.Controllers.CrudController.Base;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces;
using FinanceApp.Shared.Dto.Income;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers.CrudController
{
    [Route("[controller]")]
    [ApiController]
    public class IncomeController : CrudControllerBase<IIncomeService, IncomeDto, CreateIncome, UpdateIncome>
    {
        public IncomeController(IIncomeService service) : base(service) {}
        
    }
}