using FinanceApp.Api.Controllers.CrudController.Base;
using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Interfaces;
using FinanceApp.Shared.Dto.CurrentBalance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers.CrudSingleController
{
    [Route("[controller]")]
    [ApiController]
    public class CurrentBalanceController : CrudSingleControllerBase<ICurrentBalanceService, CurrentBalanceDto, CreateOrUpdateCurrentBalance>
    {

        public CurrentBalanceController(ICurrentBalanceService service) : base(service) 
        {
        }

        [HttpGet("Get")]
        [Authorize]
        public override async Task<IActionResult> GetAsync()
        {
            try
            {
                var resultado = await _service.GetAsync();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}