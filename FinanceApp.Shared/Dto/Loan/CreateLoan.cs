using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Dto.Loan
{
    public class CreateLoan : CreateDto
    {
        public string Name { get; set; }
        public DateTime InitialDate { get; set; }
        public int MonthsPayment { get; set; }
        public double LoanValue { get; set; }
        public double InterestRate { get; set; }
        public int Type { get; set; }

    }
}
