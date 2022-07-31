using FinanceApp.Shared;
using FinanceApp.Shared.Dto.TreasuryBond;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface ITreasuryBondService : IScopedService
    {
        Task<TreasuryBondDto> AddAsync(CreateTreasuryBond input);
        Task<Result> DeleteAsync(int id);
        Task<List<TreasuryBondDto>> GetAsync();
        Task<TreasuryBondDto> GetAsync(int id);
        Task<Result> UpdateAsync(UpdateTreasuryBond input);
    }
}