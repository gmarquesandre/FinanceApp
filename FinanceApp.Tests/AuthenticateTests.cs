using AutoMapper;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Models.CommonTables;
using FinanceApp.Tests.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Profiles;
using Xunit;

namespace FinanceApp.Tests
{
    public class AuthenticateTests : CreateDbBase
    {

        public async Task<CustomIdentityUser> ReturnDefaultUser(FinanceContext userContext)
        {
            
            var users = await userContext.Users.ToListAsync();

            return users.First();
        }

        [Fact]
        public async Task DefaultUserMustBeCreatedOnCreateContext()
        {
            var userContext = await CreateFinanceContext();
            var user = await ReturnDefaultUser(userContext);
            Assert.True(user != null);

        }      
    }
}
