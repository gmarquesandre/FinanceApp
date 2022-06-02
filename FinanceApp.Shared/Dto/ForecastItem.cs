using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Dto
{
    public class ForecastItem
    {
        public DateTime DateReference { get; set;}
        public decimal Amount { get; set;}
        public decimal CumulatedAmount { get; set;}        
        public EItemType Type { get; set; }
    }
}
