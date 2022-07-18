using FinanceApp.Shared.Models.UserTables.Bases;

namespace FinanceApp.Shared.Models.CommonTables
{
    public class WorkingDaysByYear : StandartTable
    {
        public int Year { get; set; }
        public int WorkingDays { get; set; }
        public DateTime DateLastUpdate { get; set; }
    }
}