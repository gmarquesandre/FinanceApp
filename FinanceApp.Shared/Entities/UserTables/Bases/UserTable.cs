﻿using FinanceApp.Shared.Models.CommonTables;

namespace FinanceApp.Shared.Models.UserTables.Bases
{
    public class UserTable : StandartTable
    {
        public int UserId { get; set; }
        public CustomIdentityUser User { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
    }
}