using FinanceApp.Core.Services.ForecastServices;
using FinanceApp.Shared;
using FinanceApp.Shared.Enum;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetAsync([FromQuery]DateTime currentDate)
        {
            try
            {                
                var resultado = await _service.GetForecast(currentDate, currentDate.GetLastDayInTwelveMonths(), EForecastType.Monthly, false);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetTwoWeeksForecast")]
        [Authorize]
        public async Task<IActionResult> GetTwoWeeksForecast([FromQuery] DateTime currentDate)
        {
            try
            {
                var resultado = await _service.GetForecast(currentDate, currentDate.AddDays(14), EForecastType.Daily, true);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}