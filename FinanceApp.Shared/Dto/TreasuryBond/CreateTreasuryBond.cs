namespace FinanceApp.Shared.Models
{
    public class CreateTreasuryBond
    {
        public int Type { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime InvestmentDate { get; set; }
        public DateTime CreationDateTime => DateTime.Now;
        public int Operation { get; set; }
        public decimal Quantity { get; set; }

    }
}
