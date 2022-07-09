using FinanceApp.Core.Services.UserServices.Interfaces;
using FinanceApp.Shared.Dto;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false)]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> LogaUsuarioAsync(LoginRequestDto request)
        {
            Result resultado = await _loginService.LogaUsuarioAsync(request);
            if (resultado.IsFailed) return Unauthorized(resultado.Errors);
            return Ok(resultado.Successes);
        }

        [HttpPost("ResetRequest")]
        public async Task<IActionResult> SolicitaResetSenhaUsuario(SolicitaResetDto request)
        {
            Result resultado = await _loginService.SolicitaResetSenhaUsuarioAsync(request);
            if (resultado.IsFailed) return Unauthorized(resultado.Errors);
            return Ok(resultado.Successes);
        }

        [HttpPost("Reset")]
        public IActionResult ResetaSenhaUsuario(EfetuaResetDto request)
        {
            Result resultado = _loginService.ResetaSenhaUsuario(request);
            if (resultado.IsFailed) return Unauthorized(resultado.Errors);
            return Ok(resultado.Successes);
        }
    }
}