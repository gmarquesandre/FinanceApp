using System.ComponentModel.DataAnnotations;
using UsuariosApi.Models;

namespace FinanceApp.EntityFramework.Auth
{
    public class Teste
    {
        [Key]
        public int Id { get; set; }
        public string Bla { get; set; }
        public CustomIdentityUser Usuario { get; set; }
    }
}