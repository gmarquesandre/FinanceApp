using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;
namespace FinanceApp.Shared.Models
{
    public class TreasuryBondTitle
    {        
        [Key]
        public int Id { get; set; } 
        public string Description { get; set; }
        public ETreasuryBond Type { get; set; }
        public double FixedInterestValueBuy { get; set; } 
        public double FixedInterestValueSell { get; set; } 
        public decimal UnitPriceBuy { get; set; }
        public decimal UnitPriceSell { get; set; }
        public DateTime ExpirationDate { get; set; }        
        public DateTime LastUpdateDateTime { get; set; }

        public string KeyTitle()
        {
            return string.Concat(this.Type.ToString(), this.ExpirationDate.ToString());
        }
            

    }

}
