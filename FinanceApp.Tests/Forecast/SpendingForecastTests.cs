using FinanceApp.Core.Services.CrudServices.Implementations;
using FinanceApp.Core.Services.ForecastServices.Implementations;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.UserTables;
using FinanceApp.Tests.Base;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FinanceApp.Tests.Forecast
{
    public class SpendingForecastTests : AuthenticateTests
    {

        [Fact]
        private async Task MustOpenSpendingsByTimesRecurrence()
        {


            var mapper = GetConfigurationIMapper();
            var userContext = await CreateFinanceContext();

            var user = await ReturnDefaultUser(userContext);
            int nTimes = 45;

            var newSpending = new Spending()
            {
                Amount = 10,
                Category = null,
                EndDate = null,
                Name = "Teste",
                IsEndless = false,
                IsRequired = false,
                UserId = user.Id,
                Payment = EPayment.Credit,
                CreditCard = new CreditCard()
                {
                    Name = "Teste",
                    InvoiceClosingDay = 20,
                    InvoicePaymentDay = 30,
                },
                InitialDate = DateTime.Now.Date.AddDays(-20),
                Recurrence = ERecurrence.Daily,
                TimesRecurrence = nTimes,
                CreationDateTime = DateTime.Now,
                UpdateDateTime = null,
            };

            await userContext.Spendings.AddAsync(newSpending);
            await userContext.SaveChangesAsync();

            var spendingForecast = new SpendingForecast(mapper);
            var spendingSevice = new SpendingService(userContext, mapper, spendingForecast);

            var values = await spendingSevice.GetForecast(user, EForecastType.Monthly, DateTime.Now.Date.AddMonths(12));

            Assert.True(values.Items.Count == 24);
            Assert.True(true);
        }

        //        [Fact]
        //        private async Task ShouldNotFailOnOutofRangeDateByMonthlyRecurrence()
        //        {

        //            
        //var mapper = GetConfigurationIMapper(); 
        //var userContext = await CreateFinanceContext();

        //            var user = await ReturnDefaultUser(userContext);
        //            int nTimes = 12;

        //            var newSpending = new Spending()
        //            {
        //                Amount = 10,
        //                Category = null,
        //                EndDate = null,
        //                Name = "Teste",
        //                IsEndless = false,
        //                IsRequired = false,
        //                UserId = user.Id,
        //                InitialDate = new DateTime(DateTime.Now.Year,1,31),
        //                Recurrence = ERecurrence.Monthly,
        //                TimesRecurrence = nTimes,
        //                CreationDateTime = DateTime.Now,
        //                UpdateDateTime = null,
        //            };

        //            await userContext.Spendings.AddAsync(newSpending);
        //            await userContext.SaveChangesAsync();

        //            var spendingForecast = new SpendingForecast(mapper);

        //            var values = await spendingForecast.GetSpendingsSpreadList(
        //                user, DateTime.Now.AddMonths(12));

        //            Assert.True((values.Count == 12 - DateTime.Now.Month-6) || values.Count == 12 - DateTime.Now.Month - 5);
        //            Assert.True(true);
        //        }


        //        [Fact]
        //        private async Task CreditCardPaymentTest()
        //        {

        //            
        //var mapper = GetConfigurationIMapper(); 
        //var userContext = await CreateFinanceContext();

        //            var user = await ReturnDefaultUser(userContext);
        //            int nTimes = 30;

        //            var newSpending = new Spending()
        //            {
        //                Amount = 10,
        //                Category = null,
        //                EndDate = null,
        //                Name = "Teste",
        //                IsEndless = false,
        //                IsRequired = false,
        //                UserId = user.Id,
        //                InitialDate = DateTime.Now.Date,
        //                Recurrence = ERecurrence.Daily,
        //                Payment = EPayment.Credit,
        //                CreditCard = new CreditCard()
        //                {
        //                    InvoiceClosingDay = 10,
        //                    UserId = user.Id,
        //                    Name = "Boa",
        //                    CreationDateTime = DateTime.Now,
        //                    InvoicePaymentDay = 20                     
        //                },
        //                TimesRecurrence = nTimes,
        //                CreationDateTime = DateTime.Now,
        //                UpdateDateTime = null,
        //            };

        //            await userContext.Spendings.AddAsync(newSpending);
        //            await userContext.SaveChangesAsync();


        //            var spendingSevice = new SpendingService(userContext, mapper);
        //            var spendingForecast = new SpendingForecast(mapper, spendingSevice);

        //            var spending = await userContext.Spendings.ToListAsync();
        //            var card = await userContext.CreditCards.ToListAsync();

        //            var values = await spendingForecast.GetSpendingsSpreadList(user, DateTime.Now.AddMonths(12));
        //            var dates = values.Select(a => a.Date).ToList();
        //            Assert.True(true);
        //        }


        //        [Fact]
        //        private async Task GroupedMonthValue()
        //        {

        //var mapper = GetConfigurationIMapper();
        //            var userContext = await CreateFinanceContext();

        //            var user = await ReturnDefaultUser(userContext);
        //            int nTimes = 30;

        //            var newSpending = new Spending()
        //            {
        //                Amount = 10,
        //                Category = null,
        //                EndDate = null,
        //                Name = "Teste",
        //                IsEndless = false,
        //                IsRequired = false,
        //                UserId = user.Id,
        //                InitialDate = DateTime.Now.Date,
        //                Recurrence = ERecurrence.Daily,
        //                Payment = EPayment.Credit,
        //                CreditCard = new CreditCard()
        //                {
        //                    InvoiceClosingDay = 10,
        //                    UserId = user.Id,
        //                    Name = "Boa",
        //                    CreationDateTime = DateTime.Now,
        //                    InvoicePaymentDay = 20
        //                },
        //                TimesRecurrence = nTimes,
        //                CreationDateTime = DateTime.Now,
        //                UpdateDateTime = null,
        //            };

        //            await userContext.Spendings.AddAsync(newSpending);
        //            await userContext.SaveChangesAsync();

        //            var spendingSevice = new SpendingService(userContext, mapper);
        //            var spendingForecast = new SpendingForecast(mapper, spendingSevice);

        //            var spending = await userContext.Spendings.ToListAsync();
        //            var card = await userContext.CreditCards.ToListAsync();

        //            var values = await spendingForecast.GetMonthlyForecast(user, DateTime.Now.AddMonths(12), null);

        //            Assert.True(true);
        //        }

    }
}
