using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces
{
    public interface IQuerySingle<TDto>
        where TDto : StandardDto

    {
        Task<TDto> GetAsync();
    }
}
