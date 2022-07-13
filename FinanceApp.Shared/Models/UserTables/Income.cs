using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Models.UserTables
{
    public class Income : UserTable
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public ERecurrence Recurrence { get; set; }
        public double Amount { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndless { get; set; }
        public int? TimesRecurrence { get; set; }
    }
}