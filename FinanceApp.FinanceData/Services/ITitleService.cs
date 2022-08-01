using FinanceApp.Shared;

namespace FinanceApp.FinanceData.Services
{
    public interface ITitleService 
    {
        Task<DefaultTitleOutput> GetCurrentValueOfTitle(DefaultTitleInput input);
        Task<(DefaultTitleOutput titleOutput, double withdraw)> GetCurrentTitleAfterWithdraw(DefaultTitleInput title, double withdrawValue);
    }
}