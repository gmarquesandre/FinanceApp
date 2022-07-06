using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Models.CommonTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _service;
        private readonly UserManager<CustomIdentityUser> _userManager;

        public IncomeController(IIncomeService service, UserManager<CustomIdentityUser> userManager)
        {
            _userManager = userManager;
            _service = service;
        }

        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> GetsAsync()
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
        public async Task<IActionResult> GetsAsync(int id)
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
        public async Task<IActionResult> Add(CreateIncome input)
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
        public async Task<IActionResult> Delete(int id)
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
        public async Task<IActionResult> Update(UpdateIncome input)
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