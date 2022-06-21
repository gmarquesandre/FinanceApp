using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services
{
    public partial class TitleService
    {
        public class DefaultTitleOutput
        {
            public DateTime DateInvestment { get; set; }
            public DateTime Date { get; set; }
            public double InvestmentValue { get; set; }
            public EIndex Index { get; set; }
            public double IndexPercentage { get; set; }
            public double AdditionalFixedInterest { get; set; }
            public double GrossValue { get; set; }
            public double LiquidValue { get; set; }
            public double IofValue { get; set; }
            public double IncomeTaxValue { get; set; }
        }
    }
}
