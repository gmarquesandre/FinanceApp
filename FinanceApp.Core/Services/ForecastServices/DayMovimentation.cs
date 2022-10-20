namespace FinanceApp.Core.Services.ForecastServices
{
    public class DayMovimentation
    {
            public bool UpdateBalance => MustUpdateBalance();       
            public double FGTSWithdraw { get; set; }
            public double Spendings { get; set; }
            public double LoansPayment { get; set; }
            public double IncomesReceived { get; set; }
            public double ExpiredPrivateFixedIncome { get; set; }
            public double OwingValue { get; set; }
            public double PositiveValues => FGTSWithdraw + IncomesReceived + ExpiredPrivateFixedIncome;
            public double NegativeValues => Spendings + OwingValue + LoansPayment;
            public double ResultValue => PositiveValues - NegativeValues;
            public bool PositiveBalanceDay => ResultValue >= 0.00;
            public bool NegativeBalanceDay => !PositiveBalanceDay;
            public bool ZeroChangeBalance => PositiveValues - NegativeValues < 0.01;

            private bool MustUpdateBalance()
            {
                if (PositiveValues > 0.00) return true;
                if (NegativeValues > 0.00) return true;

                return false;
            }
    }
}