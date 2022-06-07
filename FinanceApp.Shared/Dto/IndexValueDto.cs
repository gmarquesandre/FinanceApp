using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.UserTables.Bases;

namespace FinanceApp.Shared.Models.CommonTables
{
    public class IndexValueDto
    {
        public EIndex Index { get; set; }
        public string IndexName => EnumHelper<EIndex>.GetDisplayValue(Index);
        public DateTime Date { get; set; }
        public DateTime DateEnd { get; set; }
        public double Value { get; set; }
    }
}
