using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Dto.Income
{
    public class CreateIncome : CreateDto
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime InitialDate { get; set; }
        public int Recurrence { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndless { get; set; }
        public int? TimesRecurrence { get; set; }

    }
}
