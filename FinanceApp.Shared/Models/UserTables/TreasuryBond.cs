using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;
using UsuariosApi.Models;

namespace FinanceApp.Shared.Models
{
    public class TreasuryBond
    {
        [Key]
        public int Id { get; set; }
        public ETreasuryBond Type { get; set;}
        public decimal UnitPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime InvestmentDate { get; set; }
        public CustomIdentityUser User { get; set; }
        public int UserId { get; set; }
        public DateTime CreationDateTime { get; set; }        
        public DateTime UpdateDateTime { get; set; }        
        public EOperation Operation { get; set; }
        public decimal Quantity { get; set; }
        
    }
}
