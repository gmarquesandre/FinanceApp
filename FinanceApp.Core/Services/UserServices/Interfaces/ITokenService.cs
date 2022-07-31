using FinanceApp.Shared;
using FinanceApp.Shared.Entities.CommonTables;

namespace FinanceApp.Core.Services.UserServices.Interfaces
{
    public interface ITokenService : IScopedService
    {
        Token CreateToken(CustomIdentityUser usuario, string role);
    }
}