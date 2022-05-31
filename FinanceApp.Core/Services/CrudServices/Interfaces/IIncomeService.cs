using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;
using FluentResults;
using UsuariosApi.Models;

namespace FinanceApp.Core.Services
{
    public interface IIncomeService
    {
        Task<IncomeDto> AddAsync(CreateIncome input, CustomIdentityUser user);
        Task<Result> DeleteAsync(int id, CustomIdentityUser user);
        Task<List<IncomeDto>> GetAsync(CustomIdentityUser user);
        Task<IncomeDto> GetAsync(CustomIdentityUser user, int id);
        Task<Result> UpdateAsync(UpdateIncome input, CustomIdentityUser user);
    }
}