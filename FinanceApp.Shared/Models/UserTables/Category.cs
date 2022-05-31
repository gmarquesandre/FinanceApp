using FinanceApp.Shared.Models.UserTables.Bases;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Models.UserTables
{
    public class Category : UserTable
    {
        [Required]
        public string Name { get; set; }
    }
}