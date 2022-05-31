using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;
using FluentResults;
using UsuariosApi.Models;

namespace FinanceApp.Core.Services
{
    public interface ISpendingService
    {
        Task<SpendingDto> AddAsync(CreateSpending input, CustomIdentityUser user);
        Task<Result> DeleteAsync(int id, CustomIdentityUser user);
        Task<List<SpendingDto>> GetAsync(CustomIdentityUser user);
        Task<SpendingDto> GetAsync(CustomIdentityUser user, int id);
        Task<Result> UpdateAsync(UpdateSpending input, CustomIdentityUser user);
    }
}