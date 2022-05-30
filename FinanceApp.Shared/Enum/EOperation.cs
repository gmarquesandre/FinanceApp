using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Enum
{
    public enum EOperation
    {
        [Display(Name= "Compra", ShortName = "C")]
        Buy = 0,
        [Display(Name= "Venda", ShortName = "V")]
        Sell = 1,
    }
}
