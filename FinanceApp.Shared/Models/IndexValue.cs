using FinanceApp.Shared.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialAPI.Data
{
    public class IndexValue
    {
        [Key]
        public int Id { get; set; }
        public EIndex Index { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateEnd { get; set; }
        public double Value { get; set; }
    }
}
