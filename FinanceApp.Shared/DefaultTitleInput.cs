using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared
{
    public class DefaultTitleInput
    {
        public DateTime DateInvestment { get; set; }
        public double InvestmentValue { get; set; }
        public EIndex Index { get; set; }
        public ETypePrivateFixedIncome TypePrivateFixedIncome { get; set; }
        public double IndexPercentage { get; set; }
        public double AdditionalFixedInterest { get; set; }
    }
}
