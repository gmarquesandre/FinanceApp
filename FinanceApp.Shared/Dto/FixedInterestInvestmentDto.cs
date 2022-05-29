using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Dto
{
    public class FixedInterestInvestmentDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public ETypeFixedInterestInvestment Type { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public EIndex Index { get; set; }
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

        public FixedInterestInvestment First()
        {
            throw new NotImplementedException();
        }
    }
}