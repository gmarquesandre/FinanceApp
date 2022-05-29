using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UsuariosApi.Profiles;
using Xunit;

namespace FinanceApp.Tests
{
    public class AuthenticateTests : CreateDbBase
    {
        [Fact]
        public async Task DefaultUserMustBeCreatedOnCreateContext()
        {
            var myProfile = new UsuarioProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var userContext = await CreateUserDbContext();

            var users = await userContext.Users.ToListAsync();

            Assert.True(users.Count == 1);

        }      
    }
}
