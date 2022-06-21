using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Dto.FGTS
{
    public class CreateOrUpdateFGTS : UpdateDto
    {

        public double CurrentBalance { get; set; }
        public double MonthlyGrossIncome { get; set; }
        public bool AnniversaryWithdraw { get; set; }

    }
}
