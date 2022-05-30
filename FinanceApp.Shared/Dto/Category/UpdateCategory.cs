using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Models
{
    public class UpdateCategory : UpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
    }
}