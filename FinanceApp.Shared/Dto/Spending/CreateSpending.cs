using FinanceApp.Shared.Dto.Base;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Dto.Spending
{
    public class CreateSpending : CreateDto
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime InitialDate { get; set; }
        public EPayment Payment { get; set; }
        public int? CreditCardId { get; set; }
        public int Recurrence { get; set; }
        public bool IsRequired { get; set; }
        public int? Category { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndless { get; set; }
        public int? TimesRecurrence { get; set; }

    }
}
