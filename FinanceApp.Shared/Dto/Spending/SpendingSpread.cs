using FinanceApp.Shared.Dto.Category;

namespace FinanceApp.Shared.Dto.Spending
{
    public class SpendingSpread : SpendingDto
    {       
        public DateTime Date { get; set; }
        public DateTime ReferenceDate { get; set; }
    }
}
