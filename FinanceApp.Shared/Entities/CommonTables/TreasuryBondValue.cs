using FinanceApp.Shared.Entities.UserTables.Bases;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Entities.CommonTables
{
    public class TreasuryBondValue : StandardTable
    {
        public DateTime Date { get; set; }
        public ETreasuryBond Type { get; set; }
        public double FixedInterestValueBuy { get; set; }
        public double FixedInterestValueSell { get; set; }
        public double UnitPriceBuy { get; set; }
        public double UnitPriceSell { get; set; }
        public DateTime ExpirationDate { get; set; }

        public string Key()
        {
            return string.Concat(Date.ToString(), Type.ToString(), ExpirationDate.ToString());
        }


    }

}
