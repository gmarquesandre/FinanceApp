// See https://aka.ms/new-console-template for more information
using FinanceApp.Core.Importers;

Console.WriteLine("Hello, World!");



var test = new AssetImporter();
await test.GetAssets(new DateTime(2022,05,12));
