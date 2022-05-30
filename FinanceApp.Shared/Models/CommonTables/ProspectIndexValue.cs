using FinanceApp.Shared.Enum;
namespace FinanceApp.Shared.Models
{
    public class ProspectIndexValue : StandartTable
    {
        public EIndex Index { get; set; }
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
