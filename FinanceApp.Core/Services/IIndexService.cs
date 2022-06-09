using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;

namespace FinanceApp.Core.Services
{
    public interface IIndexService : IScopedService
    {
        Task<List<ProspectIndexValueDto>> GetIndexProspect(EIndex index);
        Task<List<IndexValueDto>> GetIndex(EIndex index, DateTime dateStart);
        //Task<List<TreasuryBondValue>> GetTreasuryBondLastValue();
    }
}