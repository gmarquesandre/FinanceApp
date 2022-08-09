namespace FinanceApp.Shared
{
    public static class GlobalVariables
    {



        public static double FgtsYearlyInterestRate = 0.03;
        public static double FgtsMonthlyInterestRate = 0.002466;
        public static double FgtsMonthlyGrossIncomePercentageDeposit = 0.08;
        public static double MonthsInAYear = 12.0;

        //Dividas terão por padrão 300% do CDI de juros
        public static double DefaultPercentageCdiLoan = 3.00;
    }
}
