using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Dto.CurrentBalance
{
    public class CurrentBalanceDto : StandardDto
    {
        public decimal CurrentBalance { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
    }
}