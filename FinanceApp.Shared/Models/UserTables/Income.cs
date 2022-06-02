using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.UserTables.Bases;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Models.UserTables
{
    public class Income : UserTable
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