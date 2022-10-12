using FinanceApp.Shared.Entities.UserTables.Bases;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Entities.CommonTables
{
    public class AssetEarning : StandardTable
    {
        [Required]
        public Asset Asset { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTime DeclarationDate { get; set; }
        public DateTime ExDate { get; set; }
        public double CashAmount { get; set; }
        public string Period { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        //Hash para evitar adicionar o mesmo evento duas vezes
        public string Hash { get; set; } = string.Empty;
    }
}
