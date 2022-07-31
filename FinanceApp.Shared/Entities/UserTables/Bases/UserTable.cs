using FinanceApp.Shared.Entities.CommonTables;

namespace FinanceApp.Shared.Entities.UserTables.Bases
{
    public class UserTable : StandartTable
    {
        public int UserId { get; set; }
        public CustomIdentityUser User { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
    }
}