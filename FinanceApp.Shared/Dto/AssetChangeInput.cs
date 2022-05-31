using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Dto
{
    public class AssetChangeInput
    {
        [Required]
        public string AssetCode { get; set; }
        public DateTime DateStart { get; set; }
    }
}
