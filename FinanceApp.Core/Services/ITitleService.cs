
using static FinanceApp.Core.Services.TitleService;

namespace FinanceApp.Core.Services
{
    public interface ITitleService : IScopedService
    {
        Task<DefaultTitleOutput> GetCurrentValueOfTitle(DefaultTitleInput input);
        Task<(DefaultTitleOutput titleOutput, double withdraw)> GetCurrentTitleAfterWithdraw(DefaultTitleInput title, double withdrawValue);
    }
}