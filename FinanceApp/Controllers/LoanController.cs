using FinanceApp.Shared.Dto.Loan;
using FinanceApp.Shared.Entities.CommonTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _service;
        

        public LoanController(ILoanService service)
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
        public async Task<IActionResult> AddAsync(CreateLoan input)
        {
            try
            {
                
                var result = await _service.AddAsync(input);

                return Ok(result);
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
        public async Task<IActionResult> UpdateAsync(UpdateLoan input)
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