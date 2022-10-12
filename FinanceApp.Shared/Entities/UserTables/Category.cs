using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Entities.UserTables
{
    public class Category : UserTable
    {
        [Required]
        public string Name { get; set; } = String.Empty;

        public override void CheckInput()
        {
        }
    }
}