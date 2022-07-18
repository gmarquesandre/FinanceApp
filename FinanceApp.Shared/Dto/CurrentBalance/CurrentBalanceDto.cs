using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Dto.CurrentBalance
{
    public class CurrentBalanceDto : StandardDto
    {
        public double Value { get; set; }
        public double? PercentageCdi { get; set; }
        public bool UpdateValueWithCdiIndex { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}