using FinanceApp.Shared.Dto.Base;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Dto.PrivateFixedInvestment
{
    public class CreatePrivateFixedIncome : CreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public int Index { get; set; }
        [Required]
        public bool PreFixedInvestment { get; set; }
        [Required]
        public double IndexPercentage { get; set; }
        [Required]
        public double AdditionalFixedInterest { get; set; }
        [Required]
        public DateTime InvestmentDate { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        public bool LiquidityOnExpiration { get; set; }

    }
}