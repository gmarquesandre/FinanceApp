using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Enum
{
    public enum EIndex
    {
        [Display(Name = "Selic")]
        Selic = 0,
        [Display(Name = "CDI")]
        CDI = 1,
        IPCA = 2,
        [Display(Name = "IGP-M")]
        IGPM = 3,
        [Display(Name = "TR")]
        TR = 4,
        Poupanca = 5,
        Prefixado = 6
    }
}
