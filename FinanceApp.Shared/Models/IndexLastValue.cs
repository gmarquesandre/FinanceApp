using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialAPI.Data
{
    public class IndexLastValue
    {
        [Key]
        public int Id { get; set; }
        public string IndexName { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}
