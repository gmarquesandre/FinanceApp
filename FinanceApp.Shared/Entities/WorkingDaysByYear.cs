using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Entities
{
    public class WorkingDaysByYear
    {
        [Key]
        public int Id { get; set; }
        public int Year { get; set; }
        public int WorkingDays { get; set; }
        public DateTime DateLastUpdate { get; set; }
    }
}