using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FinanceApp.EntityFramework.Auth;
using FinancialApi.WebAPI.Data;

namespace FinanceApp.Tests
{
    public class CreateDbBase
    {

        public async Task<UserDbContext> CreateUserDbContext()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
               .UseInMemoryDatabase("UserDbContext")
               .Options;

            var context = new UserDbContext(options);


            await context.Database.EnsureCreatedAsync();

            return context;
        }
        public async Task<FinanceContext> CreateDataDbContext()
        {
            var options = new DbContextOptionsBuilder<FinanceContext>()
               .UseInMemoryDatabase("FinanceContext")
               .Options;

            var context = new FinanceContext(options);
            await context.Database.EnsureCreatedAsync();


            return context;
        }
        public async Task DeleteDataDb(FinanceContext context)
        {
            await context.Database.EnsureDeletedAsync();
        }
        public async Task DeleteUserDb(UserDbContext context)
        {
            await context.Database.EnsureDeletedAsync();
        }
    }
}