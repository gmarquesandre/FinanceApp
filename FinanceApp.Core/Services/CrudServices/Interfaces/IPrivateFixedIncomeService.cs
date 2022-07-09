using FinanceApp.Shared.Dto.PrivateFixedInvestment;
using FinanceApp.Shared.Models.CommonTables;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface IPrivateFixedIncomeService : ITransientService
    {
        Task<PrivateFixedIncomeDto> AddAsync(CreatePrivateFixedIncome input);
        Task<Result> DeleteAsync(int id);
        Task<List<PrivateFixedIncomeDto>> GetAsync();
        Task<PrivateFixedIncomeDto> GetAsync(int id);
        Task<Result> UpdateAsync(UpdatePrivateFixedIncome input);
    }
}