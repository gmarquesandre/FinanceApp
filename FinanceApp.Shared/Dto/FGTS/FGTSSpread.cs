namespace FinanceApp.Shared.Dto.FGTS
{
    public class FGTSSpread : FGTSDto
    {       
        public DateTime Date { get; set; }
        public DateTime ReferenceDate { get; set; }
        public double WithdrawValue { get; set; }
        public double MonthAddValue { get; set; }
    }
}
