using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Dto.CurrentBalance
{
    public class CreateOrUpdateCurrentBalance : UpdateDto
    {

        public decimal CurrentBalance { get; set; }

    }
}
