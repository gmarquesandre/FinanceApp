using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Models.UserTables
{
    public class Loan : UserTable
    {
        public DateTime InitialDate { get; set; }
        public int MonthsPayment { get; set; }
        public string Name { get; set; }
        public double LoanValue { get; set; }
        public double InterestRate { get; set; }
        public EPaymentType Type { get; set; }
    }
}