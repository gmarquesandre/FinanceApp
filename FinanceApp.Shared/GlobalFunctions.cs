
namespace FinanceApp.Shared
{
    public static class GlobalFunctions
    {
        public static double FromYearToMonthIterestRate(this double value)
        {
            return Math.Pow(1.00 + (value / 100.00), 1.00 / GlobalVariables.MonthsInAYear) - 1;
        }

        public static DateTime GetLastDayOfThisMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        }
        public static DateTime GetLastDayInTwelveMonths(this DateTime date)
        {
            return new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1).AddMonths(13).AddDays(-1);
        }
    }
}
