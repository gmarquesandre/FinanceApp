using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Entities.UserTables.Bases;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Entities.UserTables
{
    public class PrivateFixedIncome : UserTable
    {
        [Required]
        public string Name { get; set; } = String.Empty;
        [Required]
        public ETypePrivateFixedIncome Type { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public EIndex Index { get; set; }
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

        public override void CheckInput()
        {
            if (InvestmentDate > DateTime.Now.Date)
            {
                throw new Exception("A data de investimento não pode ser maior do que hoje");
            }
            else if (InvestmentDate >= ExpirationDate)
            {
                throw new Exception("A data de vencimento deve ser maior do que a de investimento ");
            }
            else if (ExpirationDate <= DateTime.Now.Date)
            {
                throw new Exception("A data de vencimento deve ser maior do que hoje");
            }
        }
    }
}