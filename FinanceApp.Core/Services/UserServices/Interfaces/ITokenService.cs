using FinanceApp.Shared;
using FinanceApp.Shared.Entities.CommonTables;

namespace FinanceApp.Core.Services.UserServices.Interfaces
{
    public interface ITokenService 
    {
        Token CreateToken(CustomIdentityUser usuario, string role);
    }
}