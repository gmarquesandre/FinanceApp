using FinanceApp.Core.Services;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsuariosApi.Models;

namespace FinanceApp.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _service;
        private readonly UserManager<CustomIdentityUser> _userManager;

        public LoanController(ILoanService service, UserManager<CustomIdentityUser> userManager)
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
                var user = await _userManager.GetUserAsync(User);
                var resultado = await _service.GetAsync(user);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("Get/{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetInvestmentsAsync(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var resultado = await _service.GetAsync(user, id);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> AddInvestment(CreateLoan input)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var result = await _service.AddAsync(input, user);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteInvestment(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var resultado = await _service.DeleteAsync(id, user);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateInvestment(UpdateLoan input)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var resultado = await _service.UpdateAsync(input, user);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}