using FinanceApp.EntityFramework;
using FinanceApp.Shared.Models.CommonTables;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FinanceApp.Tests.Base
{
    public class AuthenticateTests : TestsBase
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
