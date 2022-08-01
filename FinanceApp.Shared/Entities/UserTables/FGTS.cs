﻿using FinanceApp.Shared.Entities.UserTables.Bases;

namespace FinanceApp.Shared.Entities.UserTables
{
    public class FGTS : UserTable
    {
        public double CurrentValue { get; set; }
        public double MonthlyGrossIncome { get; set; }
        public bool AnniversaryWithdraw { get; set; }

        public override void CheckInput()
        {
        }
    }
}