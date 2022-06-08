using Microsoft.AspNetCore.Mvc;
using FinanceApp.Shared.Enum;
using FinanceApp.Core.Services;

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

        [HttpGet("ProspectIndexes")] 
        public async Task<IActionResult> ProspectIndexesAsync([FromQuery]EIndex index)
        {
            var values = await _service.GetIndexProspect(index);
            return Ok(values);
        }

        [HttpGet("IndexValue")]
        public async Task<IActionResult> IndexesAsync([FromQuery]EIndex index, [FromQuery]DateTime dateStart)
        {
            var values = await _service.GetIndex(index, dateStart);
            return Ok(values);
        }
        
        [HttpGet("TreasuryBondLastValue")]
        public async Task<IActionResult> TreasuryBondsAsync()
        {
            var values = await _service.GetTreasuryBondLastValue();
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