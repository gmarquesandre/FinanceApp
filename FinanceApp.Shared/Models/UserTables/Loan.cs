using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Models
{
    public class Loan : UserTable
    {
        public DateTime InitialDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public decimal LoanValue { get; set; }
        public decimal InterestRate { get; set; }
        public EPaymentType Type { get; set; }
    }
}