namespace FinanceApp.Shared.Models.UserTables
{
    public class CreditCard : UserTable
    {
        public string Name { get; set; }
        public int InvoiceClosingDay { get; set; }
        public int InvoicePaymentDay { get; set; }

    }
}
