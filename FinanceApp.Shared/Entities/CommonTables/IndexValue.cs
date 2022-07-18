using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.UserTables.Bases;

namespace FinanceApp.Shared.Models.CommonTables
{
    public class IndexValue : StandartTable
    {
        public EIndex Index { get; set; }
        public EIndexRecurrence IndexRecurrence { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateEnd { get; set; }
        public double Value { get; set; }
    }
}
