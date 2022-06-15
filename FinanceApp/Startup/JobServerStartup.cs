using AutoMapper;
using FinanceApp.Core.Importers;
using FinanceApp.EntityFramework;
using Hangfire;
using Hangfire.Storage;

namespace FinanceApp.Api.Startup
{
    public static class JobServerStartup
    {
        public static void AddDefaultJobs(this IServiceProvider service)
        {
            foreach (var recurringJob in JobStorage.Current.GetConnection().GetRecurringJobs())
            {
                RecurringJob.RemoveIfExists(recurringJob.Id);
            }

            var context = new FinanceContext();

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


            var indexImporter = new IndexImporter(context);

            RecurringJob.AddOrUpdate<IndexImporter>("get-indexes", importer => importer.GetIndexes(null, null), "*/5 * * * *");

        }
    }
}
