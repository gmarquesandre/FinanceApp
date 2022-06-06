using FinanceApp.Shared.Models.CommonTables;
using System.ComponentModel.DataAnnotations;
namespace FinanceApp.Shared.Models.UserTables
{
    public class UserTable
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public CustomIdentityUser User { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
    }
}