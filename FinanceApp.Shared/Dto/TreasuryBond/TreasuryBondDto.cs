using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Models
{
    public class TreasuryBondDto
    {
        [Key]
        public int Id { get; set; }
        public ETreasuryBond Type { get; set; }
        public string TypeDisplayValue => EnumHelper<ETreasuryBond>.GetDisplayValue(Type);
        public decimal UnitPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime InvestmentDate { get; set; }
        public EOperation Operation { get; set; }
        public string OperationName => EnumHelper<EOperation>.GetDisplayValue(Operation);
        public string OperationNameShort => EnumHelper<EOperation>.GetDisplayShortValue(Operation);
        public decimal Quantity { get; set; }

    }
}