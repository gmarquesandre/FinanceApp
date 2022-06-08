using FinanceApp.Shared.Models.UserTables.Bases;

namespace FinanceApp.Shared.Models.CommonTables
{
    public class WorkingDaysByYearDto
    {
        public int Year { get; set; }
        public int WorkingDays { get; set; }
        public DateTime DateLastUpdate { get; set; }
    }
}