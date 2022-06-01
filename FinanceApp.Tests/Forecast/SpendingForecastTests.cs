using AutoMapper;
using FinanceApp.Api.Profiles;
using FinanceApp.Core;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.UserTables;
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
    }
}
