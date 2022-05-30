using FinanceApp.Core.Importers;
using FinanceApp.EntityFramework.Auth;

Console.WriteLine("boa");
var context = new FinanceContext();


//var indexImporter = new IndexProspectImporter(context);
//await indexImporter.GetProspectIndexes();


var treasuryImporter = new TreasuryBondImporter(context);
await treasuryImporter.GetLastValueTreasury();