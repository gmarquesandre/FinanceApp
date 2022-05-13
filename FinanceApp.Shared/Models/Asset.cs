using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialAPI.Data
{
    public class Asset
    { 
        [Key]
        public string AssetCodeISIN { get; set; }
        public string AssetCode { get; set; }
        public string CompanyName { get; set; }
        public double UnitPrice { get; set; }
        public DateTime DateLastUpdate { get; set; }

    }
}
