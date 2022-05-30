using FinanceApp.Core.Services;
using FinanceApp.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsuariosApi.Models;

namespace FinanceApp.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PrivateFixedIncomeController : ControllerBase
    {
        private readonly PrivateFixedIncomeService _service;
        private readonly UserManager<CustomIdentityUser> _userManager;

        public PrivateFixedIncomeController(PrivateFixedIncomeService service, UserManager<CustomIdentityUser> userManager)
        {
            _userManager = userManager;
            _service = service;
        }

        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> GetInvestmentsAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var resultado = await _service.GetAllAsync(user);
        
            return Ok(resultado);
        }


        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> GetInvestmentsAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var resultado = await _service.GetSingleAsync(user, id);

            return Ok(resultado);
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> AddInvestment(CreatePrivateFixedIncome input)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _service.AddAsync(input, user);

            return CreatedAtAction(nameof(AddInvestment), new { Id = result.Id }, result);
        }

        [HttpDelete("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteInvestment(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var resultado = await _service.DeleteAsync(id, user);

            return Ok(resultado);
        }


        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateInvestment(UpdateTreasuryBond input)
        {
            var user = await _userManager.GetUserAsync(User);
            var resultado = await _service.UpdateAsync(input, user);

            return Ok(resultado);
        }        
    }
}