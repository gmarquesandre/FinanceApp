using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.UserTables.Bases;

namespace FinanceApp.Shared.Models.CommonTables
{
    public class TreasuryBondValue : StandartTable
    {
        public DateTime Date { get; set; }
        public ETreasuryBond Type { get; set; }
        public double FixedInterestValueBuy { get; set; }
        public double FixedInterestValueSell { get; set; }
        public decimal UnitPriceBuy { get; set; }
        public decimal UnitPriceSell { get; set; }
        public DateTime ExpirationDate { get; set; }

        public string Key()
        {
            return string.Concat(Date.ToString(), Type.ToString(), ExpirationDate.ToString());
        }


    }

}
