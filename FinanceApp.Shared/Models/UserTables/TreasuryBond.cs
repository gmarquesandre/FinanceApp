using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Models
{
    public class TreasuryBond
    {
        [Key]
        public int Id { get; set; }
        public ETreasuryBond Type { get; set;}
        public decimal UnitPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreationDateTime { get; set; }        
        public EOperation Operation => EOperation.Buy;
        public decimal Quantity { get; set; }
        
    }
}
