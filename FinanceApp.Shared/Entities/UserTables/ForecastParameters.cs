using FinanceApp.Shared.Models.UserTables.Bases;

namespace FinanceApp.Shared.Models.UserTables
{
    public class ForecastParameters : UserTable
    {
        public double? PercentageCdiLoan { get; set; }
        public double? PercentageCdiFixedInteresIncometSavings { get; set; }
        public double? PercentageCdiVariableIncome { get; set; }
        // Quantos % da entrada de valores no patrimonio deve ser liquido?
        public double? SavingsLiquidPercentage { get; set; }
        public int? MonthsSavingWarning { get; set; }
    }
}

