using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.UserTables.Bases;

namespace FinanceApp.Shared.Models.CommonTables
{
    public class TreasuryBondTitle : StandartTable
    {
        public string Description { get; set; }
        public ETreasuryBond Type { get; set; }
        public double FixedInterestValueBuy { get; set; }
        public double FixedInterestValueSell { get; set; }
        public double UnitPriceBuy { get; set; }
        public double UnitPriceSell { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime LastUpdateDateTime { get; set; }

        public string KeyTitle()
        {
            return string.Concat(Type.ToString(), ExpirationDate.ToString());
        }


    }

}
