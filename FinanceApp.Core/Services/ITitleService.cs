
namespace FinanceApp.Core.Services
{
    public interface ITitleService
    {
        Task<TitleService.DefaultTitleOutput> GetCurrentValueOfTitle(TitleService.DefaultTitleInput input);
    }
}