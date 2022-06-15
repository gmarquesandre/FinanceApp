using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Enum
{
    public enum ERecurrence
    {
        [Display(Name = "Uma vez")]
        Once = 0,
        [Display(Name = "Diário")]
        Daily = 1,
        [Display(Name = "Semanal")]
        Weekly = 2,
        [Display(Name = "Mensal")]
        Monthly = 3,
        [Display(Name = "Anual")]
        Yearly = 4,
    }
}
