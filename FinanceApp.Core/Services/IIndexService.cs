using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services
{
    public interface IIndexService : ITransientService
    {
        double GetIof(int day);
        Task<List<ProspectIndexValueDto>> GetIndexProspect(EIndex index);
        Task<List<IndexValueDto>> GetIndex(EIndex index, DateTime dateStart, DateTime? endDate = null);
        Task<double> GetIndexValueBetweenDates(EIndex index, DateTime startDate, DateTime endDate, double indexPercentage);
    }
}