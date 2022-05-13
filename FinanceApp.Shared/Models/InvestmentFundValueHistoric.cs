using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialAPI.Data
{
    public class InvestmentFundValueHistoric
    {
        [Key]
        public int Id { get; set; }
        public string CNPJ { get; set; }
        public DateTime Date { get; set; }
        public double UnitPrice { get; set; }

    }
}
