using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Core.Services.CrudServices.Base
{
    public interface IForecast<TDto> 
        where TDto : StandardDto 
        
    {
        Task<List<TDto>> GetAsync();
        Task<TDto> GetAsync(int id);
    }
}
