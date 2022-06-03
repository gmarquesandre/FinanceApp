namespace FinanceApp.Shared.Dto.Loan
{
    public class LoanSpread : LoanDto
    {
        public DateTime Date {get;set;}
        public decimal LoanInterestValue { get;set;}
        public decimal LoanAmortizationValue { get; set; }
        public decimal LoanValueMonth { get; set; }
    }
}