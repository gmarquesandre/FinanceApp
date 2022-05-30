using FinanceApp.Api.Requests;
using FinanceApp.Shared.Dto;
using FluentResults;

namespace FinanceApp.Core.Services
{
    public interface ILoginService
    {
        Result LogaUsuario(LoginRequestDto request);
        Result ResetaSenhaUsuario(EfetuaResetDto request);
        Task<Result> SolicitaResetSenhaUsuarioAsync(SolicitaResetDto request);
    }
}