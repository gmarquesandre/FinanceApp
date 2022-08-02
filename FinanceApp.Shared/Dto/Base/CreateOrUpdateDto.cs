namespace FinanceApp.Shared.Dto.Base
{
    public abstract class CreateOrUpdateDto
    {
        public int Id { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
