using System.ComponentModel;

namespace FinanceApp.Shared.Enum
{
    public enum EDataCacheKey
    {
        [Description("indexes")]
        Indexes = 0,
        [Description("indexesProspect")]
        IndexesProspect = 1,
        [Description("treasuryBond")]
        TreasuryBond = 2,
        [Description("holidays")]
        Holidays = 4,
        [Description("workingDays")]
        WorkingDays = 5,
        [Description("assets")]
        Asset = 6,
        [Description("investmentFund")]
        InvestmentFund = 7,
        [Description("indexesLastValue")]
        IndexesLastValue = 8

    }
}
