namespace FinanceApp.Shared.Entities.UserTables
{
    public class CreditCard : UserTable
    {
        public string Name { get; set; } = String.Empty;
        public int InvoiceClosingDay { get; set; }
        public int InvoicePaymentDay { get; set; }

        public override void CheckInput()
        {
        }
    }
}
