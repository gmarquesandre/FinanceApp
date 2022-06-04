using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Profiles;
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
                .Where(a => !a.FullName.Contains("MapperConfiguration"))
                .ToList();

            var configuration = new MapperConfiguration(cfg =>
                {
                    profiles.ForEach(profile => cfg.AddProfile((Profile)Activator.CreateInstance(profile)));

                }
            );

            
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.ManifestModule.Name.Contains("CNDLambda")).ToList();
            //var classType = assemblies
            //    .SelectMany(s => s.GetTypes())
            //    .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
            //    .FirstOrDefault(a => (EDebitDocumentType)a.GetProperty("DebitDocumentType")?.GetValue(null, null) == documentType);



            IMapper mapper = new Mapper(configuration);
            return mapper;

        }
    }
}