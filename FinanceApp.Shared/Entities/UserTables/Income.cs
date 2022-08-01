using FinanceApp.Shared.Entities.UserTables.Bases;
using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Entities.UserTables
{
    public class Income : UserTable
    {
        [Required]
        public string Name { get; set; } = String.Empty;
        [Required]
        public ERecurrence Recurrence { get; set; }
        public double Amount { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndless { get; set; }
        public int? TimesRecurrence { get; set; }

        public override void CheckInput()
        {
            if (EndDate == null && !IsEndless && (TimesRecurrence == null || TimesRecurrence == 0))
                throw new Exception("A quantidade de repetições deve ser maior que zero para o tipo de recorrência selecionado");

            else if (Recurrence != ERecurrence.Once && EndDate == null)
                throw new Exception("A data final deve ser preenchida");

            else if (Amount <= 0.00)
                throw new Exception("O valor deve ser maior do que zero");
        }
    }
}