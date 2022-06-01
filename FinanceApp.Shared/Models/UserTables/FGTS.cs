﻿using FinanceApp.Shared.Models.UserTables.Bases;

namespace FinanceApp.Shared.Models.UserTables
{
    public class FGTS : UserTable
    {
        public decimal CurrentValue { get; set; }
        public decimal MonthlyGrossIncome { get; set; }
        public bool AnniversaryWithdraw { get; set; }
    }
}