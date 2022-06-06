using Microsoft.AspNetCore.Mvc;
using FinanceApp.Core.Services.DataServices;

namespace FinanceApp.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        public readonly IDataService _service;

        public DataController(IDataService service)
        {
            _service = service;
        } 

        [HttpGet("GetProspectIndexes")] 
        public async Task<IActionResult> GetProspectIndexesAsync()
        {
            var values = await _service.GetIndexesProspect();
            return Ok(values);
        }

        //[HttpGet("GetHolidays")]
        //public IActionResult GetHolidays()
        //{
        //    DateTime lastUpdateDateLocal = _context.Holidays.Select(a => a.DateLastUpdate).Max();

        //    var values = _context.Holidays.ToList();
        //    return Ok(values);

        //}

        //[HttpGet("GetWorkingDaysByYear")]
        //public IActionResult GetWorkingDaysByYear()
        //{

        //    var values = _context.WorkingDaysByYear.ToList();
        //    return Ok(values);

        //}

    }
}