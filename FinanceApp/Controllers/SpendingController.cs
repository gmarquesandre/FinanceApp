using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto.Spending;
using FinanceApp.Shared.Models.CommonTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SpendingController : ControllerBase
    {
        private readonly ISpendingService _service;
        private readonly UserManager<CustomIdentityUser> _userManager;

        public SpendingController(ISpendingService service, UserManager<CustomIdentityUser> userManager)
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
        public async Task<IActionResult> AddInvestment(CreateSpending input)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var resultado = await _service.AddAsync(input, user);
                return Ok(resultado);
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
        public async Task<IActionResult> UpdateInvestment(UpdateSpending input)
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