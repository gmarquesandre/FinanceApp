namespace FinanceApp.Shared.Dto.Base
{
    public class UpdateDto
    {
        public DateTime UpdateDateTime => DateTime.Now;
        public DateTime CreationDateTime { get; set; }
    }
}
