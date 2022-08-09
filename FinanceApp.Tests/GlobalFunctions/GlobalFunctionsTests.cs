using FinanceApp.Shared;
using Xunit;

namespace FinanceApp.Tests.GlobalFunctions
{
    public class GlobalFunctionsTests
    {
        [Theory]
        [InlineData(0.03, 0.002466)]
        public void TestingFromYearToMonthInterestRate(double inputValue, double resultValue)
        {
            double outputValue = inputValue.FromYearToMonthIterestRate();

            Assert.Equal(outputValue, resultValue);

        }
    }
}
