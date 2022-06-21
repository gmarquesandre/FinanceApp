using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Dto.FGTS
{
    public class FGTSDto : StandardDto
    {
        public double CurrentBalance { get; set; }
        public double MonthlyGrossIncome { get; set; }
        public bool AnniversaryWithdraw { get; set; }
    }
}