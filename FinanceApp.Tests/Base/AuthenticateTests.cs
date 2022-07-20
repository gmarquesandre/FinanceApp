using FinanceApp.EntityFramework;
using FinanceApp.Shared.Models.CommonTables;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace FinanceApp.Tests.Base
{
    public class AuthenticateTests : TestsBase
    {
        public async Task<FinanceContext> CreateFinanceContext()
        {
            var options = new DbContextOptionsBuilder<FinanceContext>()
               .UseInMemoryDatabase("FinanceContext")
               .Options;


            Mock<IHttpContextAccessor> mockContextAcessor = MockHttpContext();

            var context = new FinanceContext(options, mockContextAcessor.Object);

            await context.Database.EnsureCreatedAsync();


            return context;
        }

        
        public static Mock<IHttpContextAccessor> MockHttpContext()
        {
            Mock<IHttpContextAccessor> mockContextAcessor = new();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            mockContextAcessor.Setup(m => m.HttpContext.User).Returns(claimsPrincipal);
            return mockContextAcessor;
        }

        public async Task DeleteDataDb(FinanceContext context)
        {
            await context.Database.EnsureDeletedAsync();
        }


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
