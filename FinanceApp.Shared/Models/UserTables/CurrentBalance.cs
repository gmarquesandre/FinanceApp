using FinanceApp.Shared.Models.UserTables.Bases;

namespace FinanceApp.Shared.Models.UserTables
{
    public class CurrentBalance : UserTable
    {
        public decimal CurrentValue { get; set; }
        public decimal? PercentageCdi { get; set; }
        public bool CdIncome { get; set; }
    }
}

