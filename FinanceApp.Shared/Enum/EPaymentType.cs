using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Enum
{
    public enum EPaymentType
    {
        [Display(Name = "Sistema de Amortização Constante", ShortName = "SAC")]
        SAC = 0,
        [Display(Name = "Sistema de Amortização Price", ShortName = "PRICE")]
        PRICE = 1,
        
    }
}
