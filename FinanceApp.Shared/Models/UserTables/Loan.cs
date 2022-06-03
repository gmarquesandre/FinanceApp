using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.UserTables.Bases;

namespace FinanceApp.Shared.Models.UserTables
{
    public class Loan : UserTable
    {
        public DateTime InitialDate { get; set; }
        public int MonthsPayment { get; set; }
        public string Name { get; set; }
        public decimal LoanValue { get; set; }
        public decimal InterestRate { get; set; }
        public EPaymentType Type { get; set; }
    }
}