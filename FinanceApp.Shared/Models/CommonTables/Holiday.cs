using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Models
{
    public class Holiday
    { 
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateLastUpdate { get; set; }
    }
}
