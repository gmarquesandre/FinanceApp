namespace FinanceApp.Shared.Entities.UserTables
{
    public class FGTS : UserTable
    {
        public double CurrentBalance { get; set; }
        public double MonthlyGrossIncome { get; set; }
        public bool AnniversaryWithdraw { get; set; }
        public int MonthAniversaryWithdraw { get; set; }

        public override void CheckInput()
        {
        }
    }
}