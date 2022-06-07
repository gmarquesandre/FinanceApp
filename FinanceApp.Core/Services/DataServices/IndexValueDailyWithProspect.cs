using FinanceApp.Shared;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.DataServices
{
    public class IndexValueDailyWithProspect
    {
        public EIndex Index { get; set; }
        public EIndexRecurrence IndexRecurrence { get; set; }
        public string IndexName => EnumHelper<EIndex>.GetDisplayValue(Index);
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public bool IsProspect { get; set; }
    }
}