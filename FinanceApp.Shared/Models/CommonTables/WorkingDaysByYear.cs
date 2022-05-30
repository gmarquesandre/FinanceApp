namespace FinanceApp.Shared.Models
{
    public class WorkingDaysByYear : StandartTable
    {
        public int Year { get; set; }
        public int WorkingDays { get; set; }
        public DateTime DateLastUpdate { get; set; }
    }
}