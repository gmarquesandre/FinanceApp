using FinanceApp.Shared.Models.UserTables.Bases;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Models.CommonTables
{
    public class AssetEarning : StandartTable
    {
        [Required]
        public Asset Asset { get; set; }
        public string Type { get; set; }
        public DateTime DeclarationDate { get; set; }
        public DateTime ExDate { get; set; }
        public double CashAmount { get; set; }
        public string Period { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Notes { get; set; }
        //Hash para evitar adicionar o mesmo evento duas vezes
        public string Hash { get; set; }
    }
}
