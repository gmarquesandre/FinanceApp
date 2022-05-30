using FinanceApp.Shared.Dto.Base;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Dto
{
    public class UpdateTreasuryBond : UpdateDto
    {
        [Key]
        public int Id { get; set; }
        public int Type { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime InvestmentDate { get; set; }
        public int Operation { get; set; }
        public decimal Quantity { get; set; }
    }
}