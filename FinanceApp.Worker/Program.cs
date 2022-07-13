using AutoMapper;
using FinanceApp.Core.Importers;
using FinanceApp.EntityFramework;

Console.WriteLine("boa");
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


var indexProspect = new IndexProspectImporter(context);
await indexProspect.GetProspectIndexes();

var indexes = new IndexImporter(context);
await indexes.GetIndexes();

var treasuryBonds= new TreasuryBondImporter(context);
await treasuryBonds.GetTreasury();

