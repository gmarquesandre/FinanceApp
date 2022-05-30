using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Models
{
    public class TreasuryBond : UserTable
    {
        public ETreasuryBond Type { get; set;}
        public decimal UnitPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime InvestmentDate { get; set; }      
        public EOperation Operation { get; set; }
        public decimal Quantity { get; set; }
        
    }
}
