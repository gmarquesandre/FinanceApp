using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;
namespace FinanceApp.Shared.Models
{
    public class TreasuryBondValue
    {
        [Key]
        public int Id { get; set; }
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
