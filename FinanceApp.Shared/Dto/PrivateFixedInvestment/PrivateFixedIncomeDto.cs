using FinanceApp.Shared.Dto.Base;
using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Dto.PrivateFixedInvestment
{
    public class PrivateFixedIncomeDto : StandardDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public ETypePrivateFixedIncome Type { get; set; }
        public string TypeDisplayValue => EnumHelper<ETypePrivateFixedIncome>.GetDisplayValue(Type);
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public EIndex Index { get; set; }
        public string IndexDisplayValue => EnumHelper<EIndex>.GetDisplayValue(Index);
        [Required]
        public bool PreFixedInvestment { get; set; }
        [Required]
        public decimal IndexPercentage { get; set; }
        public decimal? AdditionalFixedInterest { get; set; }
        [Required]
        public DateTime InvestmentDate { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        public bool LiquidityOnExpiration { get; set; }
    }
}