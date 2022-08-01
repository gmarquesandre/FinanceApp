using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Core.Services.CrudServices.Base
{
    public interface IQuery<TDto> 
        where TDto : StandardDto 
        
    {
        Task<List<TDto>> GetAsync();
        Task<TDto> GetAsync(int id);
    }
}
