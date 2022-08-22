using FinanceApp.Shared;

namespace FinanceApp.FinanceData.Services
{
    public interface ITitleService 
    {
        Task<DefaultTitleOutput> GetCurrentValueOfTitle(DefaultTitleInput input, DateTime date);
        Task<(DefaultTitleOutput titleOutput, double withdraw)> GetCurrentTitleAfterWithdraw(DefaultTitleInput title, DateTime date, double withdrawValue);
    }
}