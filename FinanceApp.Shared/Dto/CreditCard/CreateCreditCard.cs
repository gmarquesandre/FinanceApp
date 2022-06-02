using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Dto.CreditCard
{
    public class CreateCreditCard : CreateDto
    {
        public string Name { get; set; }
        public int InvoiceClosingDay { get; set; }
        public int InvoicePaymentDay { get; set; }

    }
}
