using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto.FGTS;
using FinanceApp.Shared.Models.CommonTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FGTSController : ControllerBase
    {
        private readonly IFGTSService _service;
        private readonly UserManager<CustomIdentityUser> _userManager;

        public FGTSController(IFGTSService service, UserManager<CustomIdentityUser> userManager)
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

        [HttpPost("CreateOrUpdate")]
        [Authorize]
        public async Task<IActionResult> CreateOrUpdate(CreateOrUpdateFGTS input)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var resultado = await _service.AddOrUpdateAsync(input, user);
                return Ok(resultado);
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
                var user = await _userManager.GetUserAsync(User);
                var resultado = await _service.DeleteAsync(user);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}