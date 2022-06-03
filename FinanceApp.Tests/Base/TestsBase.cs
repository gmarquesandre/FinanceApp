using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Profiles;
using AutoMapper;

namespace FinanceApp.Tests.Base
{
    public class TestsBase
    {

        public async Task<FinanceContext> CreateFinanceContext()
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
        public async Task DeleteUserDb(FinanceContext context)
        {
            await context.Database.EnsureDeletedAsync();
        }

        public IMapper GetConfigurationIMapper()
        {
            var myProfile = new LoanProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);
            return mapper;

        }
    }
}