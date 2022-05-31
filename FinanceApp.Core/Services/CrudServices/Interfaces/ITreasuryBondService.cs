using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;
using FluentResults;
using UsuariosApi.Models;

namespace FinanceApp.Core.Services
{
    public interface ITreasuryBondService
    {
        Task<TreasuryBondDto> AddAsync(CreateTreasuryBond input, CustomIdentityUser user);
        Task<Result> DeleteAsync(int id, CustomIdentityUser user);
        Task<List<TreasuryBondDto>> GetAsync(CustomIdentityUser user);
        Task<TreasuryBondDto> GetAsync(CustomIdentityUser user, int id);
        Task<Result> UpdateAsync(UpdateTreasuryBond input, CustomIdentityUser user);
    }
}