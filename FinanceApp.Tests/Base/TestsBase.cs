using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FinanceApp.EntityFramework;
using AutoMapper;
using System;
using System.Linq;

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
            Type profile = typeof(Profile);
            var profiles = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(a => a.BaseType == profile)
                .Where(a => a.FullName != null && !a.FullName.Contains("MapperConfiguration"))
                .ToList();

            var configuration = new MapperConfiguration(cfg =>
                {
                    profiles.ForEach(profile => cfg.AddProfile((Profile)Activator.CreateInstance(profile)));

                }
            );

            IMapper mapper = new Mapper(configuration);
            return mapper;

        }
    }
}