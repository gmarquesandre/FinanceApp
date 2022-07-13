namespace FinanceApp.Shared.Models.UserTables
{
    public class CurrentBalance : UserTable
    {
        public double Value { get; set; }
        public double? PercentageCdi { get; set; }
        public bool UpdateValueWithCdiIndex { get; set; }
    }
}

