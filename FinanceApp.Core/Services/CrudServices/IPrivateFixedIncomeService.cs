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
        Task<List<PrivateFixedIncomeDto>> GetAllAsync(CustomIdentityUser user);
        Task<PrivateFixedIncomeDto> GetSingleAsync(CustomIdentityUser user, int id);
        Task<Result> UpdateAsync(UpdatePrivateFixedIncome input, CustomIdentityUser user);
    }
}