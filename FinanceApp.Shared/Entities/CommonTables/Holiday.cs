using FinanceApp.Shared.Models.UserTables.Bases;

namespace FinanceApp.Shared.Models.CommonTables
{
    public class Holiday : StandartTable
    {
        public DateTime Date { get; set; }
        public DateTime DateLastUpdate { get; set; }
    }
}
