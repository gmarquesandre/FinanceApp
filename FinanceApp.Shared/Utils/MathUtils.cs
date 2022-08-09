namespace FinanceApp.Shared.Utils
{
    public static class MathUtils
    {
        public static double RoundDown(this double number, int decimalPlaces)
        {
            return Math.Floor(number * Math.Pow(10, decimalPlaces)) / Math.Pow(10, decimalPlaces);
        }
    }
}
