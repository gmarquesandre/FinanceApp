//using FinanceApp.Core.Importers;
//using FinanceApp.Core.Services;
//using FinanceApp.Core.Services.CrudServices.Implementations;
//using FinanceApp.Core.Services.ForecastServices;
//using FinanceApp.Core.Services.ForecastServices.Implementations;
//using FinanceApp.Shared.Enum;
//using FinanceApp.Shared.Models.UserTables;
//using FinanceApp.Tests.Base;
//using Microsoft.Extensions.Caching.Memory;
//using System;
//using System.Threading.Tasks;
//using Xunit;

//namespace FinanceApp.Tests.Forecast
//{
//    public class LoanForecastTests : AuthenticateTests
//    {

//        [Fact]
//        private async Task ForecastTest()
//        {
//            var context = await CreateFinanceContext();

//            var mapper = GetConfigurationIMapper();

//            var user = await ReturnDefaultUser(context);


//            //Instancias 
//            MemoryCacheOptions cacheOptions = new();

//            var memoryCache = new MemoryCache(cacheOptions);

//            var datesService = new DatesService(context,
//                                    mapper,
//                                    memoryCache);

//            var indexService = new IndexService(context,
//                                                mapper,
//                                                memoryCache,
//                                                datesService);

//            var titleService = new TitleService(indexService);

//            var loanForecast = new LoanForecast(mapper);
//            var spendingForecast = new SpendingForecast(mapper);
//            var incomeForecast = new IncomeForecast(mapper);

//            var loanService = new LoanService(context, mapper, loanForecast);

//            var currentBalanceService = new CurrentBalanceService(context, mapper);

//            var spendingService = new SpendingService(context, mapper, spendingForecast);
//            var incomeService = new IncomeService(context, mapper, incomeForecast);

//            var forecastService = new ForecastService(
//                spendingService,
//                incomeService,
//                currentBalanceService,
//                loanService,
//                indexService,
//                titleService

//            );



//            //Importar dados


//            var indexesImporter = new IndexImporter(context);

//            var indexProspect = new IndexProspectImporter(context);

//            var holidaysImporter = new HolidaysImporter(context);

//            await indexProspect.GetProspectIndexes();

//            await holidaysImporter.GetHolidays(DateTime.Now.Year, DateTime.Now.Year + 1);

//            await indexesImporter.GetIndexes(indexImport: EIndex.CDI, dateStart: new DateTime(DateTime.Now.Year, 01, 01));

//            var newSpending = new Spending()
//            {
//                Amount = 10,
//                Category = null,
//                EndDate = null,
//                Name = "Teste",
//                IsEndless = false,
//                IsRequired = false,
//                UserId = user.Id,
//                Payment = EPayment.Credit,
//                CreditCard = new CreditCard()
//                {
//                    Name = "Teste",
//                    InvoiceClosingDay = 20,
//                    InvoicePaymentDay = 30,
//                },
//                InitialDate = DateTime.Now.Date.AddDays(-20),
//                Recurrence = ERecurrence.Daily,
//                TimesRecurrence = 100,
//                CreationDateTime = DateTime.Now,
//                UpdateDateTime = null,
//            };

//            await context.Spendings.AddAsync(newSpending);
//            await context.SaveChangesAsync();

//            var newIncome = new Income()
//            {
//                Amount = 1000,
//                EndDate = null,
//                Name = "Teste",
//                IsEndless = false,
//                UserId = user.Id,                
//                InitialDate = DateTime.Now.Date.AddDays(1),
//                Recurrence = ERecurrence.Monthly,
//                TimesRecurrence = 13,
//                CreationDateTime = DateTime.Now,
//                UpdateDateTime = null,
//            };

//            await context.Incomes.AddAsync(newIncome);
//            await context.SaveChangesAsync();


//            var newCurrentBalance = new CurrentBalance()
//            {
//                PercentageCdi = 1.00,
//                UpdateValueWithCdiIndex = true,
//                User = user,
//                UserId = user.Id,
//                Value = 0,
//                CreationDateTime = DateTime.Now
//            };

//            await context.CurrentBalances.AddAsync(newCurrentBalance);
//            await context.SaveChangesAsync();

//            // Teste


//            var result = await forecastService.GetForecast();


//            Assert.True(false);
//        }



//    }
//}
