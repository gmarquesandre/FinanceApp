using FinanceApp.Shared.Dto.Base;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Dto.CreditCard
{
    public class CreditCardDto : StandardDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int InvoiceClosingDay { get; set; }
        public int InvoicePaymentDay { get; set; }

    }
}