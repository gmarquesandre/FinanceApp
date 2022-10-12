using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Entities.UserTables
{
    public class TreasuryBond : UserTable
    {
        public ETreasuryBond Type { get; set; }
        public double UnitPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime InvestmentDate { get; set; }
        public EOperation Operation { get; set; }
        public double Quantity { get; set; }

        public override void CheckInput()
        {
        }
    }
}
