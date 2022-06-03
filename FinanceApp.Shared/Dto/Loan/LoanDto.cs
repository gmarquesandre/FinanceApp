using FinanceApp.Shared.Dto.Base;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Dto.Loan
{
    public class LoanDto : StandardDto
    {
        public int Id { get; set; }
        public DateTime InitialDate { get; set; }
        public int MonthsPayment { get; set; }
        public string Name { get; set; }
        public decimal LoanValue { get; set; }
        public decimal InterestRate { get; set; }
        public EPaymentType Type { get; set; }
        public string TypeDisplayValue => EnumHelper<EPaymentType>.GetDisplayValue(Type);


    }
}