// See https://aka.ms/new-console-template for more information
using FinanceApp.Core.Importers;

//var test = new AssetImporter();
//await test.GetAssets(new DateTime(2022,05,13));

//var teste = new IndexImporter();
//await teste.ImportIndexes();

var teste = new WorkingDaysImporter();
await teste.ImportWorkingDays();