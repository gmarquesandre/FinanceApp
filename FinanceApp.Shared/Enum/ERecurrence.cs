using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Enum
{
    public enum ERecurrence
    {
        [Display(Name = "Uma vez")]
        Once = 1,
        [Display(Name = "Diário")]
        Daily = 2,
        [Display(Name = "Semanal")]
        Weekly = 3,
        [Display(Name = "Mensal")]
        Monthly = 4,
        [Display(Name = "Anual")]
        Yearly = 5,
    }
}
