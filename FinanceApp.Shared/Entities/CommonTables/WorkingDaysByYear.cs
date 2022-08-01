using FinanceApp.Shared.Entities.UserTables.Bases;

namespace FinanceApp.Shared.Entities.CommonTables
{
    public class WorkingDaysByYear : Standartdable
    {
        public int Year { get; set; }
        public int WorkingDays { get; set; }
        public DateTime DateLastUpdate { get; set; }
    }
}