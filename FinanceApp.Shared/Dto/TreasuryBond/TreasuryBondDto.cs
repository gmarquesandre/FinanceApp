using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Models
{
    public class TreasuryBondDto
    {
        [Key]
        public int Id { get; set; }
        public ETreasuryBond Type { get; set; }
        private string TypeDisplayValue => EnumHelper<ETreasuryBond>.GetDisplayValue(Type);
        public decimal UnitPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime InvestmentDate { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public EOperation Operation { get; set; }
        private string OperationName => EnumHelper<EOperation>.GetDisplayValue(Operation);
        private string OperationNameShort => EnumHelper<EOperation>.GetDisplayShortValue(Operation);
        public decimal Quantity { get; set; }

    }
}