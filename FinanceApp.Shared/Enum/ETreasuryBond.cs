using FinanceApp.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Enum
{
    public enum ETreasuryBond
    {
        [Display(Name = "Tesouro Prefixado com Juros Semestrais")]
        [EIndex(EIndex.Prefixado)]
        NTNF = 0,

        [Display(Name = "Tesouro IPCA+")]
        [EIndex(EIndex.IPCA)]
        NTNBPrinc = 1,
        
        [Display(Name = "Tesouro IPCA+ Com Juros Semestrais")]
        [EIndex(EIndex.IPCA)]
        NTNB = 2,
        
        [Display(Name = "Tesouro IGPM")]
        [EIndex(EIndex.IGPM)]
        NTNC = 3,
        
        [Display(Name = "Tesouro Prefixado")]
        [EIndex(EIndex.Prefixado)]
        LTN = 4,
        
        [Display(Name = "Tesouro Selic")]
        [EIndex(EIndex.Selic)]
        LFT = 5,        
    }
}
