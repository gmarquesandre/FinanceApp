using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialAPI.Dto
{
    public class AssetChangeInput
    {
        [Required]
        public string AssetCode { get; set; }
        public DateTime DateStart { get; set; }
    }
}
