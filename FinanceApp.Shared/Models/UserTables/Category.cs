using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Models
{
    public class Category : UserTable
    {
        [Required]
        public string Name { get; set; }        
    }
}