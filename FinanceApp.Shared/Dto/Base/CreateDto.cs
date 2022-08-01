namespace FinanceApp.Shared.Dto.Base
{
    public abstract class CreateDto
    {
        public DateTime CreationDateTime => DateTime.Now;
    }
}
