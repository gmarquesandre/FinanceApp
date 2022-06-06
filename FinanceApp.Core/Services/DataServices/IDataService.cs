using FinanceApp.Shared.Models.CommonTables;

namespace FinanceApp.Core.Services.DataServices
{
    public interface IDataService : IScopedService
    {
        Task<List<ProspectIndexValue>> GetIndexesProspect();
    }
}