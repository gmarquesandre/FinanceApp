using FinanceApp.Shared.Entities.UserTables.Bases;

namespace FinanceApp.Shared.Entities.CommonTables
{
    public class Holiday : StandartTable
    {
        public DateTime Date { get; set; }
        public DateTime DateLastUpdate { get; set; }
    }
}
