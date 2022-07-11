using FinanceApp.Core.Services.ForecastServices;
using FinanceApp.Shared.Models.CommonTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ForecastController : ControllerBase
    {
        private readonly IForecastService _service;

        public ForecastController(IForecastService service)
        {
            _service = service;
        }

        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> GetInvestmentsAsync()
        {
            try
            {
                
                var resultado = await _service.GetForecast();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }     
    }
}