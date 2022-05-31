using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Models.CommonTables
{
    public class Asset
    {
        [Key]
        public int Id { get; set; }
        public string AssetCodeISIN { get; set; }
        public string AssetCode { get; set; }
        public EAssetType TypeAsset { get; set; }
        public string CompanyName { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime Date { get; set; }
        public decimal OpeningPrice { get; set; }
    }
}
