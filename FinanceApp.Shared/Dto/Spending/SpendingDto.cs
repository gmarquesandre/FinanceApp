using FinanceApp.Shared.Dto.Base;
using FinanceApp.Shared.Dto.Category;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Dto.Spending
{
    public class SpendingDto : StandardDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ERecurrence Recurrence { get; set; }
        public string RecurrenceDisplayValue => EnumHelper<ERecurrence>.GetDisplayValue(Recurrence);
        public CategoryDto? Category { get; set; }
        public bool IsRequired { get; set; }
        public decimal Amount { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndless { get; set; }
        public int TimesRecurrence { get; set; }

    }
}