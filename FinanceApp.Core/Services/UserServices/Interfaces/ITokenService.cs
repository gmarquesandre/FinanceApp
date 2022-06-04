using FinanceApp.Api.Startup;
using FinanceApp.Shared.Models.CommonTables;

namespace FinanceApp.Core.Services.UserServices.Interfaces
{
    public interface ITokenService : ITransientService
    {
        Token CreateToken(CustomIdentityUser usuario, string role);
    }
}