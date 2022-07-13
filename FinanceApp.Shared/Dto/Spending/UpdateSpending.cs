using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Dto.Spending
{
    public class UpdateSpending : UpdateDto
    {
        public string Name { get; set; }
        public int Payment { get; set; }
        public int? CreditCardId { get; set; }
        public double Amount { get; set; }
        public int Recurrence { get; set; }
        public bool IsRequired { get; set; }
        public int? CategoryId { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndless { get; set; }
        public int? TimesRecurrence { get; set; }
    }
}