using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces
{
    public interface ICrudServiceBase<TDto, TCreate, TUpdate> :
        ICommand<TDto, TCreate, TUpdate>,
        IQuery<TDto>
        where TDto : StandardDto
        where TCreate : CreateDto
        where TUpdate : UpdateDto


    {

    }
}