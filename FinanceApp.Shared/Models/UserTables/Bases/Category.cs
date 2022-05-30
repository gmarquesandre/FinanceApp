using System.ComponentModel.DataAnnotations;
using UsuariosApi.Models;

namespace FinanceApp.Shared.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public CustomIdentityUser User { get; set; }
        public int UserId { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
    }
}