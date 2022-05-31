using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Models
{
    public class CreateSpending : CreateDto
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime InitialDate { get; set; }
        public int Recurrence { get; set; }
        public bool IsRequired { get; set; }
        public int? Category { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndless { get; set; }
        public int? TimesRecurrence { get; set; }

    }
}
