using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;

namespace FinanceApp.Core.Services.DataServices
{
    public interface IDataService : IScopedService
    {
        Task<List<ProspectIndexValueDto>> GetIndexesProspect();
        Task<List<IndexValueDto>> GetIndex(EIndex index, DateTime dateStart);
        Task<List<TreasuryBondValue>> GetTreasuryBondLastValue();
    }
}