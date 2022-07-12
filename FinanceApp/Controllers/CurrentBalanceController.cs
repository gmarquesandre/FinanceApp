﻿using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto.CurrentBalance;
using FinanceApp.Shared.Models.CommonTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CurrentBalanceController : ControllerBase
    {
        private readonly ICurrentBalanceService _service;
        private readonly UserManager<CustomIdentityUser> _userManager;

        public CurrentBalanceController(ICurrentBalanceService service, UserManager<CustomIdentityUser> userManager)
        {
            _userManager = userManager;
            _service = service;
        }

        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> GetInvestmentsAsync()
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

        [HttpPost("CreateOrUpdate")]
        [Authorize]
        public async Task<IActionResult> CreateOrUpdate(CreateOrUpdateCurrentBalance input)
        {
            try
            {
                
                var resultado = await _service.AddOrUpdateAsync(input);
                return Created("", resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteInvestment()
        {
            try
            {
                
                var resultado = await _service.DeleteAsync();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}