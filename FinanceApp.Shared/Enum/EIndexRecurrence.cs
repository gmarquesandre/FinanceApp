using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Enum
{
    public enum EIndexRecurrence
    {
        [Display(Name ="Diário")]
        Daily = 0,
        [Display(Name ="Mensal")]
        Monthly = 1,
        [Display(Name ="Anual")]
        Yearly = 2,
    }
}
