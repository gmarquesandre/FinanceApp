using FinanceApp.Shared.Models.UserTables.Bases;

namespace FinanceApp.Shared.Models.UserTables
{
    public class Spending : SpendingAndIncome
    {
        public bool IsRequired { get; set; }
        public Category? Category { get; set; }
    }
}