using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Models
{
    public class InvestmentFundValue
    {
        [Key]
        public string CNPJ { get; set; }
        [MaxLength]
        public string Name { get; set; }
        [MaxLength]
        public string NameShort { get; set; }
        public string Situation { get; set; }
        public DateTime DateLastUpdate { get; set; }
        public double UnitPrice { get; set; }
        public bool TaxLongTerm { get; set; }
        public string FundTypeName { get; set; }
        public double AdministrationFee { get; set;}
        public string AdministrationFeeInfo { get; set; }
        public double PerformanceFee { get; set; }
        public string PerformanceFeeInfo { get; set; }
        public string BenchmarkIndex { get; set; }
    }
}
