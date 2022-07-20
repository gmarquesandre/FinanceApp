using FinanceApp.Core.Services.ForecastServices.Implementations;
using FinanceApp.Shared.Dto.CreditCard;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Enum;
using FinanceApp.Tests.Base;
using System;
using Xunit;

namespace FinanceApp.Tests.Forecast
{
    public class IncomeForecastTests : TestsBase
    {

        private static IncomeForecast IncomeForecastInstance()
        {
            var mapper = GetConfigurationIMapper();
            
            var IncomeForecast = new IncomeForecast(mapper);
            return IncomeForecast;
        }
        public IncomeDto Income = new IncomeDto()
        {
            Amount = 10,
            EndDate = null,
            Name = "Teste",
            IsEndless = false,
        };            

        [Fact]
        private void MustValidateSpedingsDailyByTimesRecurrence()
        {

            int nTimes = 45;           
            Income.TimesRecurrence = nTimes;
            Income.InitialDate = DateTime.Now.Date.AddDays(-20);
            Income.Recurrence = ERecurrence.Daily;

            IncomeForecast IncomeService = IncomeForecastInstance();

            var values = IncomeService.GetIncomeList(DateTime.Now.Date.AddMonths(12), DateTime.Now, Income);
            
            Assert.True(values.Count == 24);

        }

        [Fact]
        private void MustValidateSpedingsMonthlyByTimesRecurrence()
        {
            
            int nTimes = 12;
            Income.TimesRecurrence = nTimes;
            Income.InitialDate = new DateTime(DateTime.Now.Year, 1, 1);
            Income.Recurrence = ERecurrence.Monthly;
                      
            IncomeForecast IncomeService = IncomeForecastInstance();

            var values = IncomeService.GetIncomeList(DateTime.Now.AddMonths(12), DateTime.Now.Date, Income);

            Assert.True(values.Count == 12 - DateTime.Now.Month);

        }


        [Fact]
        private void MustValidateSpedingsWeeklyByTimesRecurrence()
        {
        
            int nTimes = 14;
            Income.TimesRecurrence = nTimes;
            Income.InitialDate = DateTime.Now.AddDays(1);
            Income.Recurrence = ERecurrence.Weekly;

            IncomeForecast IncomeService = IncomeForecastInstance();

            var values = IncomeService.GetIncomeList(DateTime.Now.AddMonths(12), DateTime.Now.Date, Income);

            Assert.True(values.Count == nTimes);
        }


        [Fact]
        private void MustValidateSpedingsWithCreditCardWeeklyByTimesRecurrence()
        {
            int invocieClosingDay = 10;

            int nTimes = Convert.ToInt16((new DateTime( DateTime.Now.Date.Year, DateTime.Now.Month + 2, invocieClosingDay - 1).Date - DateTime.Now.Date).TotalDays);
            Income.TimesRecurrence = nTimes;
            Income.InitialDate = DateTime.Now.Date.AddDays(1);
            Income.Recurrence = ERecurrence.Daily;
            
            IncomeForecast IncomeService = IncomeForecastInstance();

            var values = IncomeService.GetIncomeList(DateTime.Now.AddMonths(12), DateTime.Now.Date, Income);

            Assert.True(values.Count == nTimes);
        }

    }
}
