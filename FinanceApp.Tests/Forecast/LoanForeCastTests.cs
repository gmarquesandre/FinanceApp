using FinanceApp.Core.Services.ForecastServices.Implementations;
using FinanceApp.Shared.Dto.Loan;
using FinanceApp.Shared.Enum;
using FinanceApp.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FinanceApp.Tests.Forecast
{
    public class LoanForecastTests : AuthenticateTests
    {


        private static LoanForecast LoanForecastInstance()
        {
            var mapper = GetConfigurationIMapper();
            return new LoanForecast(mapper);
        }

        private List<LoanDto> ListLoan = new(){
            new LoanDto()
        {
            //Começando no mes anterior pois só com AddDays + 1 no ultimo dia do mês daria problema
            InitialDate = DateTime.Now.Date.AddMonths(-1).AddDays(-1),
            MonthsPayment = 186,
            InterestRate = 3.40,
            LoanValue = 57000.00,
            Name = "TESTE"
            },
        };
       

        [Fact]
        private void LoanPriceValueTest()
        {
            var mapper = GetConfigurationIMapper();

            ListLoan.First().Type = EPaymentType.PRICE;

            var LoanForecast = new LoanForecast(mapper);

            var values = LoanForecast.GetLoansSpreadList(ListLoan, DateTime.Now.AddMonths(200), DateTime.Now);

            double valueSecondParcel = values.First().LoanValueMonth;
            double valueThirdParcel = values.Skip(1).First().LoanValueMonth;

            Assert.True(values.Count == ListLoan.First().MonthsPayment);
            Assert.True(valueSecondParcel.ToString("N", SetPrecision) == 393.23.ToString("N", SetPrecision));
            Assert.True(valueThirdParcel.ToString("N", SetPrecision) == 393.23.ToString("N", SetPrecision));
        }
        

        [Fact]
        private void LoanSacValueTest()
        {
            
            LoanForecast loanForecast = LoanForecastInstance();
            ListLoan.First().Type = EPaymentType.SAC;

            var values = loanForecast.GetLoansSpreadList(ListLoan, DateTime.Now.AddMonths(200), DateTime.Now);

            double valueSecondParcel = values.First().LoanValueMonth;
            double valueThirdParcel = values.Skip(1).First().LoanValueMonth;
            double valueParcelLast = values.Last().LoanValueMonth;

            //Mes corrente + 184
            Assert.True(values.Count == ListLoan.First().MonthsPayment);
            Assert.True(valueSecondParcel.ToString("N", SetPrecision) == 464.63.ToString("N", SetPrecision));
            Assert.True(valueThirdParcel.ToString("N", SetPrecision) == 463.78.ToString("N", SetPrecision));
            Assert.True(valueParcelLast.ToString("N", SetPrecision) == 306.45.ToString("N", SetPrecision));

        }       

        [Fact]
        private void LoanSacValueLimitedTest()
        {
            var loanForecast = LoanForecastInstance();

            var values = loanForecast.GetLoansSpreadList(ListLoan, DateTime.Now.AddMonths(12), DateTime.Now);

            double valueSecondParcel = values.First().LoanValueMonth;
            double valueThirdParcel = values.Skip(1).First().LoanValueMonth;

            //Dia corrente + 12 meses
            Assert.True(values.Count == 13);
            Assert.True(valueSecondParcel.ToString("N", SetPrecision) == 464.63.ToString("N", SetPrecision));
            Assert.True(valueThirdParcel.ToString("N", SetPrecision) == 463.78.ToString("N", SetPrecision));

        }



    }
}
