namespace FinanceApp.Shared.Dto
{
    public class UpdateIncome
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public decimal Amount { get; set; }
        public int? CategoryId { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndless { get; set; }
        public int? TimesRecurrence { get; set; }
        public DateTime? UpdateDateTime => DateTime.Now;
    }
}