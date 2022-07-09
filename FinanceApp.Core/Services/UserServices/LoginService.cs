using FinanceApp.Core.Services.UserServices.Interfaces;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models.CommonTables;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Core.Services.UserServices
{
    public class LoginService : ILoginService
    {
        private SignInManager<CustomIdentityUser> _signInManager;
        private ITokenService _tokenService;

        public LoginService(SignInManager<CustomIdentityUser> signInManager,
            ITokenService tokenService)
        {
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<Result> LogaUsuarioAsync(LoginRequestDto request)
        {
            var resultadoIdentity = await _signInManager
                .PasswordSignInAsync(request.Username, request.Password, false, false);
            if (resultadoIdentity.Succeeded)
            {
                var identityUser = _signInManager
                    .UserManager
                    .Users
                    .FirstOrDefault(usuario =>
                    usuario.NormalizedUserName == request.Username.ToUpper());
                Token token = _tokenService
                    .CreateToken(identityUser, _signInManager
                                .UserManager.GetRolesAsync(identityUser).Result.FirstOrDefault());

                return Result.Ok().WithSuccess(token.Value);
            }
            return Result.Fail("Login falhou");
        }

        public Result ResetaSenhaUsuario(EfetuaResetDto request)
        {
            CustomIdentityUser identityUser = RecuperaUsuarioPorEmail(request.Email);

            IdentityResult resultadoIdentity = _signInManager
                .UserManager.ResetPasswordAsync(identityUser, request.Token, request.Password)
                .Result;
            if (resultadoIdentity.Succeeded) return Result.Ok()
                    .WithSuccess("Senha redefinida com sucesso!");
            return Result.Fail("Houve um erro na opera��o");
        }

        public async Task<Result> SolicitaResetSenhaUsuarioAsync(SolicitaResetDto request)
        {
            CustomIdentityUser identityUser = RecuperaUsuarioPorEmail(request.Email);

            if (identityUser != null)
            {
                string codigoDeRecuperacao = await _signInManager
                    .UserManager.GeneratePasswordResetTokenAsync(identityUser);
                return Result.Ok().WithSuccess(codigoDeRecuperacao);
            }

            return Result.Fail("Falha ao solicitar redefini��o");
        }

        private CustomIdentityUser RecuperaUsuarioPorEmail(string email)
        {
            return _signInManager
                            .UserManager
                            .Users
                            .FirstOrDefault(u => u.NormalizedEmail == email.ToUpper());
        }
    }
}