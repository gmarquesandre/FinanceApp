using FinanceApp.Shared.Dto;

namespace FinanceApp.Core.Services.DataServices
{
    public interface IDataService : IScopedService
    {
        Task<List<ProspectIndexValueDto>> GetIndexesProspect();
    }
}