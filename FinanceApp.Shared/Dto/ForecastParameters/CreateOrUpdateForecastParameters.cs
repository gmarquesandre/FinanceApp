﻿using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Dto.ForecastParameters
{
    public class CreateOrUpdateForecastParameters : UpdateDto
    {
        public double PercentageCdiLoan { get; set; }
        public double PercentageCdiFixedInteresIncometSavings { get; set; }
        public double PercentageCdiVariableIncome { get; set; }
        public double SavingsLiquidPercentage { get; set; }
        public int MonthsSavingWarning { get; set; }

    }
}