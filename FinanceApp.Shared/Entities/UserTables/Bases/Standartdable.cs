using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Entities.UserTables.Bases
{
    public class StandardTable
    {
        protected StandardTable()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
    }
}