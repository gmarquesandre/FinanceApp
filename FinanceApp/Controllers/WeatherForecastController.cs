using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinanceApp.EntityFramework;

namespace FinanceApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValueController : ControllerBase
    {
        public readonly FinanceContext _context;

        public ValueController(FinanceContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("mvp/GetProspect")]
        public IActionResult GetProspect()
        {
            var values = _context.ProspectIndexValues
                .Where(a => a.BaseCalculo == 0)
                .ToList();
            return Ok(values);
        }

        [HttpGet("mvp/GetHolidays")]
        public IActionResult GetHolidays([FromQuery] int userId)
        {
            DateTime lastUpdateDateLocal = _context.Holidays.Select(a => a.DateLastUpdate).Max();

            var values = _context.Holidays.ToList();
            return Ok(values);

        }

        [HttpGet("mvp/GetWorkingDaysByYear")]
        public IActionResult GetWorkingDaysByYear()
        {

            var values = _context.WorkingDaysByYear.ToList();
            return Ok(values);

        }

    }
}