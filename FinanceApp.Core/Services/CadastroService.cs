using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using UsuariosApi.Data.Dtos.Usuario;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class CadastroService
    {

        private IMapper _mapper;
        private UserManager<CustomIdentityUser> _userManager;
        //private EmailService _emailService;

        public CadastroService(IMapper mapper,
            UserManager<CustomIdentityUser> userManager,
            //EmailService emailService, 
            RoleManager<IdentityRole<int>> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            //_emailService = emailService;
        }

        public async Task<Result> CadastraUsuario(CreateUsuarioDto createDto)
        {
            Usuario usuario = _mapper.Map<Usuario>(createDto);
            CustomIdentityUser usuarioIdentity = _mapper.Map<CustomIdentityUser>(usuario);
            IdentityResult resultadoIdentity = await _userManager
                .CreateAsync(usuarioIdentity, createDto.Password);
            _userManager.AddToRoleAsync(usuarioIdentity, "regular");
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