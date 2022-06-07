//using FinanceApp.Core.Services.ForecastServices.Implementations;
//using FinanceApp.Shared.Dto.Loan;
//using FinanceApp.Shared.Enum;
//using FinanceApp.Tests.Base;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;

//namespace FinanceApp.Tests.Forecast
//{
//    public class LoanForecastTests : AuthenticateTests
//    {

//        [Fact]
//        private void LoanPriceValueTest()
//        {
//            var mapper = GetConfigurationIMapper();

//            var list = new List<LoanDto>(){
//                new LoanDto()
//            {
//                InitialDate = DateTime.Now.Date,
//                MonthsPayment = 186,
//                InterestRate = 3.40M,
//                LoanValue = 57000.00M,
//                Name = "FIES",
//                Type = EPaymentType.PRICE
//             },
//            };
//            NumberFormatInfo setPrecision = new();
//            setPrecision.NumberDecimalDigits = 2;

//            var LoanForecast = new LoanForecast(mapper);

//            var values = LoanForecast.GetLoanSpreadList(list, DateTime.Now.AddMonths(12));

//            decimal valueParcel = values.First().LoanValueMonth;

//            Assert.True(valueParcel.ToString("N", setPrecision) == 393.23.ToString("N", setPrecision));
//        }


//        [Fact]
//        private async Task LoanSacValueTest()
//        {
//            var mapper = GetConfigurationIMapper();

//            var list = new List<LoanDto>(){
//                new LoanDto()
//            {
//                InitialDate = DateTime.Now.Date,
//                MonthsPayment = 186,
//                InterestRate = 3.40M,
//                LoanValue = 57000.00M,
//                Name = "TESTE",
//                Type = EPaymentType.SAC
//             },
//            };

//            NumberFormatInfo setPrecision = new();
//            setPrecision.NumberDecimalDigits = 2;

//            var LoanForecast = new LoanForecast(mapper);

//            var values = LoanForecast.GetLoanSpreadList(list, DateTime.Now.AddMonths(200));

//            decimal valueParcelFirst = values.First().LoanValueMonth;
//            decimal valueParcelLast = values.Last().LoanValueMonth;

//            Assert.True(valueParcelFirst.ToString("N", setPrecision) == 465.49.ToString("N", setPrecision));
//            Assert.True(valueParcelLast.ToString("N", setPrecision) == 307.31.ToString("N", setPrecision));

//        }

//    }
//}
