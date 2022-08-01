using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Entities
{
    public class InvestmentFundValue
    {
        [Key]
        public string CNPJ { get; set; } = string.Empty;
        [MaxLength]
        public string Name { get; set; } = string.Empty;
        [MaxLength]
        public string NameShort { get; set; } = string.Empty;
        public string Situation { get; set; } = string.Empty;
        public DateTime DateLastUpdate { get; set; }
        public double UnitPrice { get; set; }
        public bool TaxLongTerm { get; set; }
        public string FundTypeName { get; set; } = string.Empty;
        public double AdministrationFee { get; set; }
        public string AdministrationFeeInfo { get; set; } = string.Empty;
        public double PerformanceFee { get; set; }
        public string PerformanceFeeInfo { get; set; } = string.Empty;
        public string BenchmarkIndex { get; set; } = string.Empty;
    }
}
