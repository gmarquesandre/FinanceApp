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
            var user = await _userManager.GetUserAsync(User);
            var resultado = await _service.GetAsync(user);
        
            return Ok(resultado);
        }


        [HttpGet("Get/{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetInvestmentsAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var resultado = await _service.GetAsync(user, id);

            return Ok(resultado);
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> AddInvestment(CreateSpending input)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _service.AddAsync(input, user);

            return CreatedAtAction(nameof(AddInvestment), new { Id = result.Id }, result);
        }

        [HttpDelete("Delete/{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteInvestment(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var resultado = await _service.DeleteAsync(id, user);

            return Ok(resultado);
        }


        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateInvestment(UpdateSpending input)
        {
            var user = await _userManager.GetUserAsync(User);
            var resultado = await _service.UpdateAsync(input, user);

            return Ok(resultado);
        }        
    }
}