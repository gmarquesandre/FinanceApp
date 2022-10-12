using FinanceApp.Shared.Entities.UserTables.Bases;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Entities.CommonTables
{
    public class IndexValue : StandardTable
    {
        public EIndex Index { get; set; }
        public EIndexRecurrence IndexRecurrence { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateEnd { get; set; }
        public double Value { get; set; }
    }
}
