using System;
using System.ComponentModel.DataAnnotations;
namespace FinancialAPI.Data
{
    public class ProspectIndexValue
    {
        [Key]
        public int Id { get; set; }
        public string IndexName { get; set; }
        public DateTime DateResearch { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd{ get; set; }
        public double Median { get; set; }
        public double Average { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public int ResearchAnswers { get; set; }
        public int BaseCalculo { get; set; }
        public DateTime DateLastUpdate { get; set; }
    }
}
