using FinanceApp.Core.Services;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using UsuariosApi.Data.Dtos.Usuario;

namespace FinanceApp.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CadastroController : ControllerBase
    {
        private CadastroService _cadastroService;

        public CadastroController(CadastroService cadastroService)
        {
            _cadastroService = cadastroService;
        }
      
        [HttpGet("/teste")]
        public async Task<IActionResult> GetTeste()
        {
            
            return Ok(await _cadastroService.GetTeste());

        }
        [HttpPost]
        public async Task<IActionResult> CadastraUsuarioAsync(CreateUsuarioDto createDto)
        {
            Result resultado = await _cadastroService.CadastraUsuario(createDto);
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