using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Base;
using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces
{
    public interface ICrudSingleBase<TDto, TCreateOrUpdate> :
        ICommandSingle<TDto, TCreateOrUpdate>,
        IQuerySingle<TDto>
        where TDto : StandardDto
        where TCreateOrUpdate : CreateOrUpdateDto


    {

    }
}