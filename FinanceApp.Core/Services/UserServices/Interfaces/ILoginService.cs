using FinanceApp.Shared;
using FinanceApp.Shared.Dto;
using FluentResults;

namespace FinanceApp.Core.Services.UserServices.Interfaces
{
    public interface ILoginService : IScopedService
    {
        Task<Result> LogaUsuarioAsync(LoginRequestDto request);
        Result ResetaSenhaUsuario(EfetuaResetDto request);
        Task<Result> SolicitaResetSenhaUsuarioAsync(SolicitaResetDto request);
    }
}