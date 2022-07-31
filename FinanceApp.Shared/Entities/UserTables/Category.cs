using FinanceApp.Shared.Entities.UserTables.Bases;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Entities.UserTables
{
    public class Category : UserTable
    {
        [Required]
        public string Name { get; set; }
    }
}