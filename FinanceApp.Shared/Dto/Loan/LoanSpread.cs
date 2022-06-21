namespace FinanceApp.Shared.Dto.Loan
{
    public class LoanSpread : LoanDto
    {
        public DateTime Date {get;set;}
        public double LoanInterestValue { get;set;}
        public double LoanAmortizationValue { get; set; }
        public double LoanValueMonth { get; set; }
    }
}