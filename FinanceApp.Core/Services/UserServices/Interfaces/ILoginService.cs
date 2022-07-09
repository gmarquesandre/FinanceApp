﻿using FinanceApp.Shared.Dto;
using FluentResults;

namespace FinanceApp.Core.Services.UserServices.Interfaces
{
    public interface ILoginService : ITransientService
    {
        Task<Result> LogaUsuarioAsync(LoginRequestDto request);
        Result ResetaSenhaUsuario(EfetuaResetDto request);
        Task<Result> SolicitaResetSenhaUsuarioAsync(SolicitaResetDto request);
    }
}