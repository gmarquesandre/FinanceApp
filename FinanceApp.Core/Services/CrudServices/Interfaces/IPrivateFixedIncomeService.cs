using FinanceApp.Shared.Dto.PrivateFixedInvestment;
using FinanceApp.Shared.Models.CommonTables;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface IPrivateFixedIncomeService : ITransientService
    {
        Task<PrivateFixedIncomeDto> AddAsync(CreatePrivateFixedIncome input, CustomIdentityUser user);
        Task<Result> DeleteAsync(int id, CustomIdentityUser user);
        Task<List<PrivateFixedIncomeDto>> GetAsync(CustomIdentityUser user);
        Task<PrivateFixedIncomeDto> GetAsync(CustomIdentityUser user, int id);
        Task<Result> UpdateAsync(UpdatePrivateFixedIncome input, CustomIdentityUser user);
    }
}