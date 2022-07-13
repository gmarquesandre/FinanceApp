using FinanceApp.Shared.Dto.Base;
using FinanceApp.Shared.Dto.Category;
using FinanceApp.Shared.Dto.CreditCard;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Dto.Spending
{
    public class SpendingDto : StandardDto
    {
        public string Name { get; set; }
        public ERecurrence Recurrence { get; set; }
        public EPayment Payment { get; set; }
        public CreditCardDto? CreditCard { get; set; }
        public string RecurrenceDisplayValue => EnumHelper<ERecurrence>.GetDisplayValue(Recurrence);
        public string PaymentName => EnumHelper<EPayment>.GetDisplayValue(Payment);
        public CategoryDto? Category { get; set; }
        public bool IsRequired { get; set; }
        public double Amount { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndless { get; set; }
        public int TimesRecurrence { get; set; }

    }
}