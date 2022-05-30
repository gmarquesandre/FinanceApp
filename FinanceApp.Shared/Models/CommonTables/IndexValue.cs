using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Models
{
    public class IndexValue : StandartTable
    {
        public EIndex Index { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateEnd { get; set; }
        public double Value { get; set; }
    }
}
