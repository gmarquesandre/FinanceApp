using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Dto.Loan
{
    public class UpdateLoan : UpdateDto
    {
        public DateTime InitialDate { get; set; }
        public int MonthsPayment { get; set; }
        public string Name { get; set; }
        public double LoanValue { get; set; }
        public double InterestRate { get; set; }
        public int Type { get; set; }
    }
}