using AutoMapper;
using FinanceApp.EntityFramework.Auth;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using UsuariosApi.Data.Dtos.Usuario;
using UsuariosApi.Models;

namespace FinanceApp.Core.Services
{
    public class UserRegisterService : IUserRegisterService
    {

        private IMapper _mapper;
        private UserManager<CustomIdentityUser> _userManager;
        private FinanceContext _teste;
        //private EmailService _emailService;

        public UserRegisterService(IMapper mapper,
            UserManager<CustomIdentityUser> userManager,
            FinanceContext teste)
        {
            _mapper = mapper;
            _teste = teste;
            _userManager = userManager;
            //_emailService = emailService;
        }

        public async Task<Result> UserRegister(CreateUsuarioDto createDto)
        {
            Usuario usuario = _mapper.Map<Usuario>(createDto);

            CustomIdentityUser usuarioIdentity = _mapper.Map<CustomIdentityUser>(usuario);

            IdentityResult resultadoIdentity = await _userManager
                .CreateAsync(usuarioIdentity, createDto.Password);

            await _userManager.AddToRoleAsync(usuarioIdentity, "regular");


            if (resultadoIdentity.Succeeded)
            {
                var code = await _userManager
                    .GenerateEmailConfirmationTokenAsync(usuarioIdentity);
                //var encodedCode = HttpUtility.UrlEncode(code);

                //_emailService.EnviarEmail(new[] { usuarioIdentity.Email },
                //    "Link de Ativação", usuarioIdentity.Id, encodedCode);

                return Result.Ok().WithSuccess(code);
            }
            return Result.Fail("Falha ao cadastrar usuário");

        }

        //public Result AtivaContaUsuario(AtivaContaRequest request)
        //{
        //    var identityUser = _userManager
        //        .Users
        //        .FirstOrDefault(u => u.Id == request.UsuarioId);
        //    var identityResult = _userManager
        //        .ConfirmEmailAsync(identityUser, request.CodigoDeAtivacao).Result;
        //    if (identityResult.Succeeded)
        //    {
        //        return Result.Ok();
        //    }
        //    return Result.Fail("Falha ao ativar conta de usuário");
        //}
    }
}