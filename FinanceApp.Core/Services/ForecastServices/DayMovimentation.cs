namespace FinanceApp.Core.Services.ForecastServices
{
    public class DayMovimentation
    {
            public bool UpdateBalance => MustUpdateBalance();       
            public double FGTSWithdraw { get; set; }
            public double Spendings { get; set; }
            public double LoansPayment { get; set; }
            public double IncomesReceived { get; set; }
            public double OwingValue { get; set; }
            public double PositiveValues => FGTSWithdraw + IncomesReceived;
            public double NegativeValues => Spendings + OwingValue + LoansPayment;
            public double ResultValue => PositiveValues - NegativeValues;
            public bool PositiveBalanceDay => ResultValue >= 0.00;
            public bool NegativeBalanceDay => !PositiveBalanceDay;
            public bool ZeroChangeBalance => PositiveValues == NegativeValues;

            private bool MustUpdateBalance()
            {
                if (FGTSWithdraw > 0.00) return true;
                if (Spendings > 0.00) return true;
                if (LoansPayment > 0.00) return true;
                if (IncomesReceived > 0.00) return true;
                if (IncomesReceived > 0.00) return true;
                if (OwingValue > 0.00) return true;

                return false;
            }
    }
}