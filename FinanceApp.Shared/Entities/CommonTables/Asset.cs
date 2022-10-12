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
        public double ClosingPrice { get; set; }
        public double OpeningPrice { get; set; }
        public double MaxPrice { get; set; }
        public double MinPrice { get; set; }
        public double AveragePrice { get; set; }
        public double TradeCount { get; set; }
        public double StockTradeCount { get; set; }
        public DateTime Date { get; set; }
    }
}
