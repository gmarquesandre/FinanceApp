using FinanceApp.Shared.Entities.UserTables.Bases;

namespace FinanceApp.Shared.Entities.CommonTables
{
    public class AssetChange : Standartdable
    {
        public Asset Asset { get; set; } = new Asset();
        public string Type { get; set; } = string.Empty;
        public DateTime DeclarationDate { get; set; }
        public DateTime ExDate { get; set; }
        public double GroupingFactor { get; set; }
        public string ToAssetISIN { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        //Hash para evitar adicionar o mesmo evento duas vezes
        public string Hash { get; set; } = string.Empty;
    }
}
