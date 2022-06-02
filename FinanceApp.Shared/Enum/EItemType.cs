using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Enum
{
    public enum EItemType
    {
        [Display(Name = "Gasto")]
        Spending = 0,
        [Display(Name = "Renda")]
        Income = 1,
        [Display(Name = "Empréstimos/Financiamentos")]
        Loan = 2,
        [Display(Name = "Renda Fixa")]
        PrivateFixedIncomeInvestment = 3,
        [Display(Name = "Tesouro Direto")]
        TreasuryBond = 4,
        [Display(Name = "FGTS")]
        FGTS = 5,
        [Display(Name = "Conta Corrente")]
        CurrentBalance = 6,

    }
}
