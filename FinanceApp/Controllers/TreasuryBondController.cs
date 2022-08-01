using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto.TreasuryBond;
using FinanceApp.Shared.Entities.CommonTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TreasuryBondController : ControllerBase
    {
        private readonly ITreasuryBondService _service;
        

        public TreasuryBondController(ITreasuryBondService service)
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


        [HttpGet("Get/{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                
                var resultado = await _service.GetAsync( id);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> AddAsync(CreateTreasuryBond input)
        {
            try
            {
                
                var resultado = await _service.AddAsync(input);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                
                var resultado = await _service.DeleteAsync(id);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(UpdateTreasuryBond input)
        {
            try
            {
                
                var resultado = await _service.UpdateAsync(input);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}