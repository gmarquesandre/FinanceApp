using FinanceApp.Shared.Entities.CommonTables;
using FinanceApp.Shared.Entities.UserTables.Bases;

namespace FinanceApp.Shared.Entities.UserTables
{
    public abstract class UserTable : Standartdable
    {
        public int UserId { get; set; }
        public CustomIdentityUser User { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public abstract void CheckInput();
    }
}