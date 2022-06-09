namespace FinanceApp.Core.Services
{
    public class Iof
    {
        public Iof(int day, double value)
        {
            Day = day;
            Value = value;
        }
        public int Day { get; set; }
        public double Value { get; set; }
    }
}
