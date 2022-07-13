namespace FinanceApp.Shared.Models.UserTables
{
    public class FGTS : UserTable
    {
        public double CurrentValue { get; set; }
        public double MonthlyGrossIncome { get; set; }
        public bool AnniversaryWithdraw { get; set; }
    }
}