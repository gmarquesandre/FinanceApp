namespace FinanceApp.Shared.Models
{
    public class Spending : SpendingAndIncome
    {
        public bool IsRequired { get; set; }
        public Category? Category { get; set; }
    }
}