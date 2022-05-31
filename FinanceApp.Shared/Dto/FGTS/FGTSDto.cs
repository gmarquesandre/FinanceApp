﻿namespace FinanceApp.Shared.Dto.FGTS
{
    public class FGTSDto
    {
        public decimal CurrentBalance { get; set; }
        public decimal MonthlyGrossIncome { get; set; }
        public bool AnniversaryWithdraw { get; set; }
    }
}