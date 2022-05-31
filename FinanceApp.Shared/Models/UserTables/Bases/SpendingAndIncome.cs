using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Models.UserTables.Bases
{
    public class SpendingAndIncome : UserTable
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public ERecurrence Recurrence { get; set; }
        public decimal Amount { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndless { get; set; }
        public int? TimesRecurrence { get; set; }
    }
}