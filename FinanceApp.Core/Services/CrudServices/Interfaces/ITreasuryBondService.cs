using FinanceApp.Shared.Dto.TreasuryBond;
using FinanceApp.Shared.Models.CommonTables;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface ITreasuryBondService : ITransientService
    {
        Task<TreasuryBondDto> AddAsync(CreateTreasuryBond input);
        Task<Result> DeleteAsync(int id);
        Task<List<TreasuryBondDto>> GetAsync();
        Task<TreasuryBondDto> GetAsync(int id);
        Task<Result> UpdateAsync(UpdateTreasuryBond input);
    }
}