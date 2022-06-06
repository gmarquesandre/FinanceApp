using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Enum
{
    public enum EForecastType
    {

        [Display(Name = "Mensal")]
        Daily = 0,
        [Display(Name = "Anual")]
        Monthly = 1
    }
}
