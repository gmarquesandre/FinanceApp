using FinanceApp.Core.Importers;
using FinanceApp.EntityFramework;

Console.WriteLine("boa");
var context = new FinanceContext();

var indexProspect = new IndexProspectImporter(context);
await indexProspect.GetProspectIndexes();

var indexes = new IndexImporter(context);
await indexes.GetIndexes();

