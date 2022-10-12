using FinanceApp.Shared.Entities.UserTables.Bases;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Entities.CommonTables
{
    public class ProspectIndexValue : StandardTable
    {
        public EIndex Index { get; set; }
        public EIndexRecurrence IndexRecurrence { get; set; }
        public DateTime DateResearch { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public double Median { get; set; }
        public double Average { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public int ResearchAnswers { get; set; }
        public int BaseCalculo { get; set; }
        public DateTime DateLastUpdate { get; set; }
    }

}
