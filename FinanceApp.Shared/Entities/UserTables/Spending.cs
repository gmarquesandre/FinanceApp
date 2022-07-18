using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;
using FinanceApp.Shared.Models.UserTables.Bases;

namespace FinanceApp.Shared.Models.UserTables
{
    public class Spending : UserTable
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public ERecurrence Recurrence { get; set; }
        public double Amount { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndless { get; set; }
        public int? TimesRecurrence { get; set; }
        public bool IsRequired { get; set; }
        public Category? Category { get; set; }
        public EPayment Payment { get; set; }
        public CreditCard? CreditCard { get; set; } // Cartão de Crédito
        public int? CreditCardId { get; set; }
    }
}
