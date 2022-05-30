using FluentResults;
using UsuariosApi.Data.Dtos.Usuario;

namespace FinanceApp.Core.Services
{
    public interface IUserRegisterService
    {
        Task<Result> UserRegister(CreateUsuarioDto createDto);
    }
}