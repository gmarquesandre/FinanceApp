using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces
{
    public interface IQuery<TDto>
        where TDto : StandardDto

    {
        Task<List<TDto>> GetAsync();
        Task<TDto> GetAsync(int id);
    }
}
