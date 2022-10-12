namespace FinanceApp.Shared.Dto.Base
{
    public abstract class UpdateDto
    {
        public Guid Id { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public DateTime CreationDateTime { get; set; }        
    }
}
