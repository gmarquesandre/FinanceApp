using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Enum
{
    public enum EPayment
    {
        [Display(Name ="Dinheiro / Cartão de Débito")]
        Cash = 0,
        [Display(Name ="Cartão de Crédito")]
        Credit = 1
    }
}
