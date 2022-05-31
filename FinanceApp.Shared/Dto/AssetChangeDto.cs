using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Dto
{
    public class AssetChangeDto
    {
        [Key]
        public int Id { get; set; }
        public string AssetCode { get; set; }
        public string AssetCodeISIN { get; set; }
        public string Type { get; set; }
        public DateTime DeclarationDate { get; set; }
        public DateTime ExDate { get; set; }
        public double GroupingFactor { get; set; }
        public string ToAssetISIN { get; set; }
        public string Notes { get; set; }
        public string Hash { get; set; }
    }
}
