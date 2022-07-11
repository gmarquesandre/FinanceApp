using FinanceApp.Api;
using FinanceApp.Shared.Models.CommonTables;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApp.Shared.Models.UserTables
{
    public class UserTable
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public CustomIdentityUser User { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreationDateTime { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdateDateTime { get; set; }
    }
}