using FinanceApp.Shared.Enum;
namespace FinanceApp.Shared.Models
{
    public class TreasuryBondTitle
    {
        public ETreasuryBond Type { get; set; }
        public double FixedInterestValueBuy { get; set; }
        public double FixedInterestValueSell { get; set; }
        public double UnitPriceBuy { get; set; }
        public double UnitPriceSell { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public string KeyTitle()
        {
            return string.Concat(Type.ToString(), ExpirationDate.ToString());
        }


    }

}
