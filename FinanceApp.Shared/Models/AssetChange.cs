using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialAPI.Data
{
    public class AssetChange
    { 
        [Key]
        public int Id { get; set; }
        public Asset Asset { get; set; }
        public string Type { get; set; }
        public DateTime DeclarationDate { get; set; }
        public DateTime ExDate { get; set; }
        public double GroupingFactor { get; set; }
        public string ToAssetISIN{ get; set; }
        public string Notes { get; set; }
        //Hash para evitar adicionar o mesmo evento duas vezes
        public string Hash { get; set; }
    }
}
