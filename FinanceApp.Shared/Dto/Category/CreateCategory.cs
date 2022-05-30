using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Models
{
    public class CreateCategory : CreateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}