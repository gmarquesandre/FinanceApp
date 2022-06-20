using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Dto.CurrentBalance
{
    public class CurrentBalanceDto : StandardDto
    {
        public decimal Value { get; set; }
        public decimal? PercentageCdi { get; set; }
        public bool UpdateValueWithCdiIndex { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
    }
}