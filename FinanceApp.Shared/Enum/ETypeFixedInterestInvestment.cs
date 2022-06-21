using FinanceApp.Shared.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApp.Shared.Enum
{
    public enum ETypePrivateFixedIncome
    {
        [NotMapped]
        [Display(Name = "Conta Corrente CDI")]
        [TypeInvestmentTax(ETypeInvestmentTax.BalanceTax)]
        BalanceCDB = 0,

        [Display(Name = "CDB - Certificado de Depósito Bancário")]
        [TypeInvestmentTax(ETypeInvestmentTax.DefaultInvestmentTax)]
        CDB = 1,

        [Display(Name = "LCA - Letras de Crédito do Agronegócio")]
        [TypeInvestmentTax(ETypeInvestmentTax.NotApplied)]
        LCA = 2,

        [Display(Name = "Letras de Crédito Imobiliário ", ShortName = "LCI")]
        [TypeInvestmentTax(ETypeInvestmentTax.NotApplied)]        
        LCI = 3,

        [Display(Name = "Letras de Câmbio", ShortName = "LC")]
        [TypeInvestmentTax(ETypeInvestmentTax.DefaultInvestmentTax)]
        LC = 4,

        [Display(Name = "Certificado de Recebíveis do Agronegócio", ShortName = "CRA")]
        [TypeInvestmentTax(ETypeInvestmentTax.NotApplied)]
        CRA = 5,

        [Display(Name = "Certificado de Recebíveis Imobiliarios", ShortName = "CRA")]
        [TypeInvestmentTax(ETypeInvestmentTax.NotApplied)]
        CRI = 6
    }
}
