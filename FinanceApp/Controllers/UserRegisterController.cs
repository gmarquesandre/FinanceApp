using FinanceApp.Core.Services;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using UsuariosApi.Data.Dtos.Usuario;

namespace FinanceApp.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserRegisterController : ControllerBase
    {
        private IUserRegisterService _cadastroService;

        public UserRegisterController(IUserRegisterService cadastroService)
        {
            _cadastroService = cadastroService;
        }
      
        [HttpPost]
        public async Task<IActionResult> CadastraUsuarioAsync(CreateUsuarioDto createDto)
        {
            Result resultado = await _cadastroService.UserRegister(createDto);
            if (resultado.IsFailed) return StatusCode(500);
            return Ok(resultado.Successes);
        }

        //[HttpGet("/ativa")]
        //public IActionResult AtivaContaUsuario([FromQuery] AtivaContaRequest request)
        //{
        //    Result resultado = _cadastroService.AtivaContaUsuario(request);
        //    if (resultado.IsFailed) return StatusCode(500);
        //    return Ok(resultado.Successes);
        //}
    }
}