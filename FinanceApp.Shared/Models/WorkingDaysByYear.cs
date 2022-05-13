
using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialAPI.Data
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