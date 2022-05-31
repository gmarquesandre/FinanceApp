using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Models
{
    public class CreateOrUpdateFGTS : UpdateDto
    {

        public decimal CurrentBalance { get; set; }
        public decimal MonthlyGrossIncome { get; set; }
        public bool AnniversaryWithdraw { get; set; }

    }
}
