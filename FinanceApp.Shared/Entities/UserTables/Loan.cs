using FinanceApp.Shared.Entities.UserTables.Bases;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Entities.UserTables
{
    public class Loan : UserTable
    {
        public DateTime InitialDate { get; set; }
        public int MonthsPayment { get; set; }
        public string Name { get; set; } = String.Empty;
        public double LoanValue { get; set; }
        public double InterestRate { get; set; }
        public EPaymentType Type { get; set; }

        public override void CheckInput()
        {
        }
    }
}