using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Base
{
    public interface ICrudSingleBase<TDto, TCreateOrUpdate> :
        ICommandSingle<TDto, TCreateOrUpdate>,
        IQuerySingle<TDto>
        where TDto : StandardDto
        where TCreateOrUpdate : CreateOrUpdateDto


    {

    }
}