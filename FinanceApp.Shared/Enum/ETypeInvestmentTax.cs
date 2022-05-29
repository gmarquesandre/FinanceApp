namespace FinanceApp.Shared.Enum
{
    public enum ETypeInvestmentTax
    {
        NotApplied = 0,
        
        //Até 180 dias  22.5%
        //Entre 181 e 360 dias 20.0
        //Entre 361 dias e 720 17.5%
        // >= 720  dias 15.0%
        DefaultInvestmentTax = 1,
        
    }
}
