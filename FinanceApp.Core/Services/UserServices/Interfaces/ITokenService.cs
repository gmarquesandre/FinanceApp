using FinanceApp.Shared.Models;
using UsuariosApi.Models;

namespace FinanceApp.Core.Services
{
    public interface ITokenService
    {
        Token CreateToken(CustomIdentityUser usuario, string role);
    }
}