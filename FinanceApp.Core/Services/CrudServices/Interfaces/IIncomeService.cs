using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Models.CommonTables;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface IIncomeService : ITransientService
    {
        Task<IncomeDto> AddAsync(CreateIncome input, CustomIdentityUser user);
        Task<Result> DeleteAsync(int id, CustomIdentityUser user);
        Task<List<IncomeDto>> GetAsync(CustomIdentityUser user);
        Task<IncomeDto> GetAsync(CustomIdentityUser user, int id);
        Task<Result> UpdateAsync(UpdateIncome input, CustomIdentityUser user);
    }
}