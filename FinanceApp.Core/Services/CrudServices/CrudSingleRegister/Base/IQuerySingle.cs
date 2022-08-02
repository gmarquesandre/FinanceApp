using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Base
{
    public interface IQuerySingle<TDto>
        where TDto : StandardDto

    {
        Task<TDto> GetAsync();
    }
}
