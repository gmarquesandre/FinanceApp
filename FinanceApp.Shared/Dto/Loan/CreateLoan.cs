using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Dto.Loan
{
    public class CreateLoan : CreateDto
    {
        public DateTime InitialDate { get; set; }
        public int MonthsPayment { get; set; }
        public string Name { get; set; }
        public decimal LoanValue { get; set; }
        public decimal InterestRate { get; set; }
        public int Type { get; set; }

    }
}
