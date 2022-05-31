using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.UserTables.Bases;

namespace FinanceApp.Shared.Models.UserTables
{
    public class TreasuryBond : UserTable
    {
        public ETreasuryBond Type { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime InvestmentDate { get; set; }
        public EOperation Operation { get; set; }
        public decimal Quantity { get; set; }

    }
}
