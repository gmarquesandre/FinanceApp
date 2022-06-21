using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services
{
    public partial class TitleService
    {
        public class DefaultTitleInput
        {
            public DateTime DateInvestment { get; set; }
            public DateTime Date { get; set; }
            public double InvestmentValue { get; set; }
            public EIndex Index { get; set; }
            public ETypePrivateFixedIncome TypePrivateFixedIncome { get; set; }
            public double IndexPercentage { get; set; }
            public double AdditionalFixedInterest { get; set; }

        }
    }
}
