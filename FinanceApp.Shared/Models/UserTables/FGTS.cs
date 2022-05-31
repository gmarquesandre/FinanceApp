namespace FinanceApp.Shared.Models
{
    public class FGTS : UserTable
    {
        public decimal CurrentBalance { get; set; }        
        public decimal MonthlyGrossIncome { get; set; }        
        public bool AnniversaryWithdraw { get; set; }        
    }
}