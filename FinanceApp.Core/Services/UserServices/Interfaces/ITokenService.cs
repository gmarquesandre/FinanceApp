using FinanceApp.Shared.Models.CommonTables;

namespace FinanceApp.Core.Services.UserServices.Interfaces
{
    public interface ITokenService
    {
        Token CreateToken(CustomIdentityUser usuario, string role);
    }
}