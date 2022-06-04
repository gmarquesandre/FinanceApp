using FinanceApp.Api.Startup;
using FinanceApp.Shared.Dto.TreasuryBond;
using FinanceApp.Shared.Models.CommonTables;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface ITreasuryBondService : ITransientService
    {
        Task<TreasuryBondDto> AddAsync(CreateTreasuryBond input, CustomIdentityUser user);
        Task<Result> DeleteAsync(int id, CustomIdentityUser user);
        Task<List<TreasuryBondDto>> GetAsync(CustomIdentityUser user);
        Task<TreasuryBondDto> GetAsync(CustomIdentityUser user, int id);
        Task<Result> UpdateAsync(UpdateTreasuryBond input, CustomIdentityUser user);
    }
}