using AutoMapper;
using FinanceApp.Core.Importers;
using Hangfire;
using FinanceApp.EntityFramework;

namespace FinanceApp.Api.Startup
{
    public static class JobServerStartup
    {
        public static void AddDefaultJobs(this IServiceProvider service)
        {
            //foreach (var recurringJob in JobStorage.Current.GetConnection().GetRecurringJobs())
            //{
            //    RecurringJob.RemoveIfExists(recurringJob.Id);
            //}

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

            RecurringJob.AddOrUpdate<HolidaysImporter>("get-holidays", importer => importer.GetHolidays(2000, DateTime.Now.Year + 40), Cron.Yearly);
            RecurringJob.AddOrUpdate<AssetImporter>("get-asserts", importer => importer.GetAssets(), Cron.Yearly);
            RecurringJob.AddOrUpdate<IndexImporter>("get-indexes", importer => importer.GetIndexes(null, null), "0 0 23 * * ?");
            RecurringJob.AddOrUpdate<IndexProspectImporter>("get-indexes-prospect", importer => importer.GetProspectIndexes(), "0 0 23 * * ?");
            RecurringJob.AddOrUpdate<TreasuryBondImporter>("get-treasury-bond", importer => importer.GetLastValueTreasury(), "0 0 23 * * ?");
            RecurringJob.AddOrUpdate<TreasuryBondImporter>("get-treasury", importer => importer.GetTreasury(), Cron.Yearly);

        }
    }
}
