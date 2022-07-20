using FinanceApp.Core.Services.ForecastServices.Implementations;
using FinanceApp.Shared.Dto.CreditCard;
using FinanceApp.Shared.Dto.Spending;
using FinanceApp.Shared.Enum;
using FinanceApp.Tests.Base;
using System;
using Xunit;

namespace FinanceApp.Tests.Forecast
{
    public class SpendingForecastTests : TestsBase
    {

        private static SpendingForecast SpendingForecastInstance()
        {
            var mapper = GetConfigurationIMapper();
            
            var spendingForecast = new SpendingForecast(mapper);
            return spendingForecast;
        }
        public SpendingDto Spending = new SpendingDto()
        {
            Amount = 10,
            Category = null,
            EndDate = null,
            Name = "Teste",
            IsEndless = false,
            IsRequired = false,
        };            

        [Fact]
        private void MustValidateSpedingsDailyByTimesRecurrence()
        {

            int nTimes = 45;           
            Spending.TimesRecurrence = nTimes;
            Spending.Payment = EPayment.Cash;
            Spending.InitialDate = DateTime.Now.Date.AddDays(-20);
            Spending.Recurrence = ERecurrence.Daily;

            SpendingForecast spendingService = SpendingForecastInstance();

            var values = spendingService.GetSpendingList(DateTime.Now.Date.AddMonths(12), DateTime.Now, Spending);
            
            Assert.True(values.Count == 24);

        }

        [Fact]
        private void MustValidateSpedingsMonthlyByTimesRecurrence()
        {
            
            int nTimes = 12;
            Spending.TimesRecurrence = nTimes;
            Spending.Payment = EPayment.Cash;
            Spending.InitialDate = new DateTime(DateTime.Now.Year, 1, 1);
            Spending.Recurrence = ERecurrence.Monthly;
                      
            SpendingForecast spendingService = SpendingForecastInstance();

            var values = spendingService.GetSpendingList(DateTime.Now.AddMonths(12), DateTime.Now.Date, Spending);

            Assert.True(values.Count == 12 - DateTime.Now.Month);

        }


        [Fact]
        private void MustValidateSpedingsWeeklyByTimesRecurrence()
        {
        
            int nTimes = 14;
            Spending.TimesRecurrence = nTimes;
            Spending.Payment = EPayment.Cash;
            Spending.InitialDate = DateTime.Now.AddDays(1);
            Spending.Recurrence = ERecurrence.Weekly;

            SpendingForecast spendingService = SpendingForecastInstance();

            var values = spendingService.GetSpendingList(DateTime.Now.AddMonths(12), DateTime.Now.Date, Spending);

            Assert.True(values.Count == nTimes);
        }


        [Fact]
        private void MustValidateSpedingsWithCreditCardWeeklyByTimesRecurrence()
        {
            int invocieClosingDay = 10;

            int nTimes = Convert.ToInt16((new DateTime( DateTime.Now.Date.Year, DateTime.Now.Month + 2, invocieClosingDay - 1).Date - DateTime.Now.Date).TotalDays);
            Spending.TimesRecurrence = nTimes;
            Spending.Payment = EPayment.Credit;
            Spending.InitialDate = DateTime.Now.Date.AddDays(1);
            Spending.Recurrence = ERecurrence.Daily;
            Spending.CreditCard = new CreditCardDto()
            {
                InvoiceClosingDay = invocieClosingDay,
                Name = "Boa",
                InvoicePaymentDay = 20
            };            
        
            SpendingForecast spendingService = SpendingForecastInstance();

            var values = spendingService.GetSpendingList(DateTime.Now.AddMonths(12), DateTime.Now.Date, Spending);

            Assert.True(values.Count == nTimes);
        }

    }
}
