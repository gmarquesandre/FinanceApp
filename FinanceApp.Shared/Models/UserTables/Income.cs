namespace FinanceApp.Shared.Models
{
    public class Income : SpendingAndIncome
    {
        public SpendingCategory? Category { get; set; }
    }
}