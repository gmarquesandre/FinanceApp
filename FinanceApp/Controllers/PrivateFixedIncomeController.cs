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
            var resultado = await _service.GetAllFixedIncomeAsync(user);
        
            return Ok(resultado);
        }


        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> AddInvestment(CreatePrivateFixedIncome input)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _service.AddInvestmentAsync(input, user);

            return CreatedAtAction(nameof(AddInvestment), new { Id = result.Id }, result);
        }

        [HttpDelete("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteInvestment(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var resultado = await _service.DeleteInvestmentAsync(id, user);

            return Ok(resultado);
        }


        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateInvestment(UpdatePrivateFixedIncome input)
        {
            var user = await _userManager.GetUserAsync(User);
            var resultado = await _service.UpdateInvestmentAsync(input, user);

            return Ok(resultado);
        }        
    }
}