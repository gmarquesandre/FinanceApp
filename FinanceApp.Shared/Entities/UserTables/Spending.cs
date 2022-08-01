using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;
using FinanceApp.Shared.Entities.UserTables.Bases;

namespace FinanceApp.Shared.Entities.UserTables
{
    public class Spending : UserTable
    {
        [Required]
        public string Name { get; set; } = string.Empty;
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

        public override void CheckInput()
        {
            if (Payment == EPayment.Credit && CreditCard == null)
                throw new Exception("Pagamentos em Crédito devem ser vinculados a um cartão");

            if (EndDate == null && !IsEndless && (TimesRecurrence == null || TimesRecurrence == 0))
                throw new Exception("A quantidade de repetições deve ser maior que zero para o tipo de recorrência selecionado");

            else if (Recurrence != ERecurrence.Once && EndDate == null)
                throw new Exception("A data final deve ser preenchida");

            else if (Amount <= 0.00)
                throw new Exception("O valor deve ser maior do que zero");
        }
    }
}
