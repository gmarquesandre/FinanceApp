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
        public string OperationName => EnumHelper<EOperation>.GetDisplayValue(Operation);
        public string OperationNameShort => EnumHelper<EOperation>.GetDisplayShortValue(Operation);
        public decimal Quantity { get; set; }

    }
}

//using FinanceApp.Shared.Enum;
//using System.ComponentModel.DataAnnotations;

//namespace FinanceApp.Shared.Models
//{
//    public class TreasuryBondDto
//    {
//        [Key]
//        public int Id { get; set; }
//        [Required]
//        public string Name { get; set; }
//        [Required]
//        public ETypePrivateFixedIncome Type { get; set; }
//        private string TypeDisplayValue => EnumHelper<ETypePrivateFixedIncome>.GetDisplayValue(Type);
//        [Required]
//        public decimal Amount { get; set; }
//        [Required]
//        public EIndex Index { get; set; }
//        private string IndexDisplayValue => EnumHelper<EIndex>.GetDisplayValue(Index);
//        [Required]
//        public bool PreFixedInvestment { get; set; }
//        [Required]
//        public decimal IndexPercentage { get; set; }
//        public decimal? AdditionalFixedInterest { get; set; }
//        [Required]
//        public DateTime InvestmentDate { get; set; }
//        [Required]
//        public DateTime ExpirationDate { get; set; }
//        [Required]
//        public bool LiquidityOnExpiration { get; set; }
//    }
//}