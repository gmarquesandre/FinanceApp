using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Dto.TreasuryBond
{
    public class CreateTreasuryBond : CreateDto
    {
        public int Type { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime InvestmentDate { get; set; }
        public int Operation { get; set; }
        public decimal Quantity { get; set; }

    }
}
