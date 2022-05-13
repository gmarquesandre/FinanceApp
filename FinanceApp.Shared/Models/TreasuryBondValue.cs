using System;
using System.ComponentModel.DataAnnotations;
namespace FinancialAPI.Data
{
    public class TreasuryBondValue
    {
        [Key]
        public string CodeISIN { get; set; }
        public string TreasuryBondName { get; set; }
        public string IndexName { get; set; }
        public double FixedInterestValueBuy { get; set; } 
        public double FixedInterestValueSell { get; set; } 
        public DateTime DateLastUpdate { get; set; }
        public double UnitPriceBuy { get; set; }
        public double UnitPriceSell { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime LastAvailableDate { get; set; }

    }
}
