using AutoMapper;
using FinanceApp.Core.Services.Forecast.Implementations;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.UserTables;
using FinanceApp.Shared.Profiles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FinanceApp.Tests.Forecast
{
    public class SpendingForecastTests : AuthenticateTests
    {

        [Fact]
        private async Task MustOpenSpendingsByTimesRecurrence()
        {
            
            var myProfile = new SpendingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);
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
                InitialDate = DateTime.Now.Date.AddDays(-20),
                Recurrence = ERecurrence.Daily,
                TimesRecurrence = nTimes,
                CreationDateTime = DateTime.Now,
                UpdateDateTime = null,                                
            };

            await userContext.Spendings.AddAsync(newSpending);
            await userContext.SaveChangesAsync();
            var spendingForecast = new SpendingForecast(mapper, userContext);

            var values = await spendingForecast.GetSpendingsSpreadList(DateTime.Now.AddMonths(1),
                user);

            Assert.True(values.Count == 24);
            Assert.True(true);
        }

        [Fact]
        private async Task ShouldNotFailOnOutofRangeDateByMonthlyRecurrence()
        {

            var myProfile = new SpendingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);
            var userContext = await CreateFinanceContext();

            var user = await ReturnDefaultUser(userContext);
            int nTimes = 12;

            var newSpending = new Spending()
            {
                Amount = 10,
                Category = null,
                EndDate = null,
                Name = "Teste",
                IsEndless = false,
                IsRequired = false,
                UserId = user.Id,
                InitialDate = new DateTime(DateTime.Now.Year,1,31),
                Recurrence = ERecurrence.Monthly,
                TimesRecurrence = nTimes,
                CreationDateTime = DateTime.Now,
                UpdateDateTime = null,
            };

            await userContext.Spendings.AddAsync(newSpending);
            await userContext.SaveChangesAsync();
            var spendingForecast = new SpendingForecast(mapper, userContext);

            var values = await spendingForecast.GetSpendingsSpreadList(DateTime.Now.AddMonths(12),
                user);

            Assert.True((values.Count == 12 - DateTime.Now.Month-6) || values.Count == 12 - DateTime.Now.Month - 5);
            Assert.True(true);
        }


        [Fact]
        private async Task CreditCardPaymentTest()
        {

            var myProfile = new SpendingProfile();
            var creditProfile = new CreditCardProfile();
            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile(myProfile); cfg.AddProfile(creditProfile); });
            IMapper mapper = new Mapper(configuration);
            var userContext = await CreateFinanceContext();

            var user = await ReturnDefaultUser(userContext);
            int nTimes = 30;

            var newSpending = new Spending()
            {
                Amount = 10,
                Category = null,
                EndDate = null,
                Name = "Teste",
                IsEndless = false,
                IsRequired = false,
                UserId = user.Id,
                InitialDate = DateTime.Now.Date,
                Recurrence = ERecurrence.Daily,
                Payment = EPayment.Credit,
                CreditCard = new CreditCard()
                {
                    InvoiceClosingDay = 10,
                    UserId = user.Id,
                    Name = "Boa",
                    CreationDateTime = DateTime.Now,
                    InvoicePaymentDay = 20                     
                },
                TimesRecurrence = nTimes,
                CreationDateTime = DateTime.Now,
                UpdateDateTime = null,
            };

            await userContext.Spendings.AddAsync(newSpending);
            await userContext.SaveChangesAsync();
            var spendingForecast = new SpendingForecast(mapper, userContext);

            var spending = await userContext.Spendings.ToListAsync();
            var card = await userContext.CreditCards.ToListAsync();

            var values = await spendingForecast.GetSpendingsSpreadList(DateTime.Now.AddMonths(12),
                user);
            var dates = values.Select(a => a.Date).ToList();
            Assert.True(true);
        }
    }
}
