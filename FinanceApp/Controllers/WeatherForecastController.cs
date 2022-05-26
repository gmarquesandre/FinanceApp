using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using FinancialApi.WebAPI.Data;
using FinancialAPI.Dto;
using System.Collections.Generic;
using FinancialAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace FinancialAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class ValueController : ControllerBase
    {
        public readonly FinanceContext _context;

        public ValueController(FinanceContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("mvp/GetProspect")]
        public ActionResult GetProspect()
        {
            var values = _context.ProspectIndexValues
                .Where(a => a.BaseCalculo == 0)
                .ToList();
            return Ok(values);
        }

        [HttpGet("mvp/GetHolidays")]
        public ActionResult GetHolidays([FromQuery] int userId)
        {
            DateTime lastUpdateDateLocal = _context.Holidays.Select(a => a.DateLastUpdate).Max();
            
             var values = _context.Holidays.ToList();
             return Ok(values);
            
        }

        [HttpGet("mvp/GetWorkingDaysByYear")]
        public ActionResult GetWorkingDaysByYear()
        {
            
            var values = _context.WorkingDaysByYear.ToList();
            return Ok(values);
        
        }

    }
}