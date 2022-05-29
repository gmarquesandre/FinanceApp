using FinanceApp.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Enum
{
    public enum ETreasuryBond
    {
        [Display(Name = "Tesouro Prefixado com Juros Semestrais")]
        [Index(EIndex.Prefixado)]
        NTNF = 0,

        [Display(Name = "Tesouro IPCA+")]
        [Index(EIndex.IPCA)]
        NTNBPrinc = 1,
        
        [Display(Name = "Tesouro IPCA+ Com Juros Semestrais")]
        [Index(EIndex.IPCA)]
        NTNB = 2,
        
        [Display(Name = "Tesouro IGPM+ Com Juros Semestrais")]
        [Index(EIndex.IGPM)]
        NTNC = 3,
        
        [Display(Name = "Tesouro Prefixado")]
        [Index(EIndex.Prefixado)]
        LTN = 4,
        
        [Display(Name = "Tesouro Selic")]
        [Index(EIndex.Selic)]
        LFT = 5,        
    }
}
