
namespace FinanceApp.Shared
{
    public static class GlobalFunctions
    {
        public static double FromYearToMonthIterestRate(this double value)
        {
            return Math.Pow(1.00 + (value / 100.00), 1.00 / GlobalVariables.MonthsInAYear) - 1;
        }
    }
}
