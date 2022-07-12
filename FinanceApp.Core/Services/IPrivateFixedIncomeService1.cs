using FinanceApp.Shared.Dto.PrivateFixedInvestment;
using FluentResults;

namespace FinanceApp.Core.Services
{
    public interface IPrivateFixedIncomeService1
    {
        Task<PrivateFixedIncomeDto> AddInvestmentAsync(CreatePrivateFixedIncome input);
        Task<Result> DeleteInvestmentAsync(int id);
        Task<List<PrivateFixedIncomeDto>> GetAllFixedIncomeAsync();
        Task<Result> UpdateInvestmentAsync(UpdatePrivateFixedIncome input);
    }
}