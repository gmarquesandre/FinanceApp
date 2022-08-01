using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Entities.CommonTables
{
    public class Asset
    {
        [Key]
        public int Id { get; set; }
        public string AssetCodeISIN { get; set; } = string.Empty;
        public string AssetCode { get; set; } = string.Empty;
        public EAssetType TypeAsset { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public double UnitPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
