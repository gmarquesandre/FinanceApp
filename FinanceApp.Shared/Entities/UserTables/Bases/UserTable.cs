using FinanceApp.Shared.Entities.CommonTables;

namespace FinanceApp.Shared.Entities.UserTables.Bases
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