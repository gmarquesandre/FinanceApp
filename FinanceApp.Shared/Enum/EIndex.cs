using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Enum
{
    public enum EIndex
    {
        [Display(Name = "Selic")]
        Selic = 0,
        [Display(Name = "CDI")]
        CDI = 1,
        [Display(Name = "IPCA")]
        IPCA = 2,
        [Display(Name = "IGP-M")]
        IGPM = 3,
        [Display(Name = "TR")]
        TR = 4,
        [Display(Name = "Poupanca")]
        Poupanca = 5,
        [Display(Name = "Prefixado")]
        Prefixado = 6
    }
}
