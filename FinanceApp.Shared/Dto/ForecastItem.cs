namespace FinanceApp.Shared.Dto
{
    public class ForecastItem
    {
        public DateTime DateReference { get; set;}
        public double NominalAmount { get; set;}
        public double LiquidValue { get; set; }
        public double NotLiquidValue { get; set; }
        public double RealAmount { get; set;}   
        public double CumulatedAmount { get; set;}
    }
}
