using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialAPI.Data
{
    public class IndexValue
    {
        [Key]
        public int Id { get; set; }
        public string IndexName { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}
