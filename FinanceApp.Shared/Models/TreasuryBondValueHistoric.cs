using System;
using System.ComponentModel.DataAnnotations;
namespace FinancialAPI.Data
{
    public class TreasuryBondValueHistoric
    {
        [Key]
        public int Id { get; set; }
        public string CodeISIN { get; set; }
        public string TreasuryBondName { get; set; }
        public DateTime Date { get; set; }
        public double FixedInterestValueBuy { get; set; }
        public double FixedInterestValueSell { get; set; }
        public double UnitPriceBuy { get; set; }
        public double UnitPriceSell { get; set; }
    }
}
