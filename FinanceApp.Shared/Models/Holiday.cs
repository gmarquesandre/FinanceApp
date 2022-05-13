using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialAPI.Data
{
    public class Holiday
    { 
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateLastUpdate { get; set; }
        public string Type { get; set; }
        public string StateCode { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
    }
}
