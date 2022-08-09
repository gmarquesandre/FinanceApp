using FinanceApp.Core.Services.ForecastServices.Implementations;
using FinanceApp.FinanceData.Services;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.CreditCard;
using FinanceApp.Shared.Dto.FGTS;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Enum;
using FinanceApp.Tests.Base;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FinanceApp.Tests.Forecast
{
    public class FGTSForecastTests : TestsBase
    {

        private static FGTSForecast FgtsForecastInstance()
        {
            var mapper = GetConfigurationIMapper();
           
            var IncomeForecast = new FGTSForecast(mapper);
            return IncomeForecast;
        }
        public FGTSDto FgtsDefault = new()
        {
            AnniversaryWithdraw = false,
            CurrentBalance = 0.00,
            MonthAniversaryWithdraw = 11,
            MonthlyGrossIncome = 0,
            UpdateDateTime = DateTime.Now,
        };


        [Fact]
        public Task MustValidateFgtsWithdrawAsync()
        {
            var fgtsForecastInstance = FgtsForecastInstance();

            var fgtsDto = new FGTSDto
            {
                UpdateDateTime = new DateTime(2021, 10, 1),
                MonthlyGrossIncome = 3775.38,
                AnniversaryWithdraw = true,
                MonthAniversaryWithdraw = 11,
                CurrentBalance = 8582.69
            };

            var fgtsSpreadList = fgtsForecastInstance.GetFGTSsSpreadListAsync(fgtsDto, new DateTime(2021, 11, 30), new DateTime(2021, 10, 01));

            var fgtsSpreadLastItem = fgtsSpreadList.Last();

            Assert.Equal(fgtsSpreadLastItem.WithdrawValue.ToString("N", SetPrecision), 2366.53.ToString("N", SetPrecision));
            Assert.Equal(fgtsSpreadLastItem.CurrentBalance.ToString("N", SetPrecision), 6533.51.ToString("N", SetPrecision));
            return Task.CompletedTask;
        }
    }
}
