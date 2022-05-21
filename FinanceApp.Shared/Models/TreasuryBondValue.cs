using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;
namespace FinancialAPI.Data
{
    public class TreasuryBondValue
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CodeISIN { get; set; }
        public ETreasuryBond Type { get; set; }
        public double FixedInterestValueBuy { get; set; } 
        public double FixedInterestValueSell { get; set; } 
        public double UnitPriceBuy { get; set; }
        public double UnitPriceSell { get; set; }
        public DateTime ExpirationDate { get; set; }        
    }

}
