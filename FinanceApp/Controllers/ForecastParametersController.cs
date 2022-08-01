﻿using FinanceApp.Core.Services.CrudServices.CrudSingleRegister;
using FinanceApp.Shared.Dto.ForecastParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ForecastParametersController : ControllerBase
    {
        private readonly IForecastParametersService _service;
        

        public ForecastParametersController(IForecastParametersService service)
        {

            _service = service;
        }

        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> GetAsync()
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
        public async Task<IActionResult> CreateOrUpdate(CreateOrUpdateForecastParameters input)
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
        public async Task<IActionResult> DeleteAsync()
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