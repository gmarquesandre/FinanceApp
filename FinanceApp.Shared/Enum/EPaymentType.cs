using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Enum
{
    public enum EPaymentType
    {
        [Display(Name = "Sistema de Amortização Constante", ShortName = "SAC")]
        SAC = 1,
        [Display(Name = "Sistema de Amortização Price", ShortName = "PRICE")]
        PRICE = 2,
        [Display(Name = "Sistema de Amortização Crescente", ShortName = "SACRE")]
        SACRE = 3,
        
    }
}
