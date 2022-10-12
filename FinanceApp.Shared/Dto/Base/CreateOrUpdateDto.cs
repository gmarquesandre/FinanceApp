namespace FinanceApp.Shared.Dto.Base
{
    public abstract class CreateOrUpdateDto
    {
        public CreateOrUpdateDto()
        {
            Id = new Guid();
        }
        public Guid Id { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
