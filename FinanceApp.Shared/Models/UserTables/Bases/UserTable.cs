using System.ComponentModel.DataAnnotations;
using UsuariosApi.Models;

namespace FinanceApp.Shared.Models
{
    public class UserTable : StandartTable
    {
        public int UserId { get; set; }
        public CustomIdentityUser User { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
    }
}