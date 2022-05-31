using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;
using FluentResults;
using UsuariosApi.Models;

namespace FinanceApp.Core.Services
{
    public interface IPrivateFixedIncomeService
    {
        Task<PrivateFixedIncomeDto> AddAsync(CreatePrivateFixedIncome input, CustomIdentityUser user);
        Task<Result> DeleteAsync(int id, CustomIdentityUser user);
        Task<List<PrivateFixedIncomeDto>> GetAsync(CustomIdentityUser user);
        Task<PrivateFixedIncomeDto> GetAsync(CustomIdentityUser user, int id);
        Task<Result> UpdateAsync(UpdatePrivateFixedIncome input, CustomIdentityUser user);
    }
}